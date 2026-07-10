using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PersonsApp.Constants;
using PersonsApp.Database;
using PersonsApp.Dtos.Auth;
using PersonsApp.Dtos.Common;
using PersonsApp.Entities;

namespace PersonsApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PersonsDbContext _context;

        public AuthService(
            SignInManager<UserEntity> signInManager,
            UserManager<UserEntity> userManager,
            IConfiguration configuration,
            PersonsDbContext context
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                isPersistent : false,
                lockoutOnFailure : false
            );
            if(!result.Succeeded)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Datos no validos"
                };
            }
            var UserEntity = await _userManager.FindByEmailAsync(dto.Email);
            List<Claim> authClaims = await GetClaimsAsync(UserEntity);

            var refreshToken = GenerateRefreshTokenString();
            UserEntity.RefreshToken = refreshToken;
            UserEntity.RefreshTokenExpiry = DateTime
            .Now.AddMinutes(int.Parse(_configuration["JWT:RefreshTokenExpiry"]));

            await _context.SaveChangesAsync();
            var jwToken = GetToken(authClaims);

            return new ResponseDto<LoginResponseDto>
            {
              StatusCode = HttpStatusCode.OK,
              Status = true,
              Message = "Autenticacion Satisfactoria",
              Data = new LoginResponseDto
              {
                  Email = UserEntity.Email,
                  Token = new JwtSecurityTokenHandler().WriteToken(jwToken),
                  RefreshToken = refreshToken
              }  
            };

        }

        public Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            throw new NotImplementedException();
            
        }
        private async Task<List<Claim>> GetClaimsAsync(UserEntity userEntity)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", userEntity.Id)

            };
            var userRoles = await _userManager.GetRolesAsync(userEntity);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            return authClaims;
        }
                private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];
            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken GetToken(List<Claim> authclaims)
        {
            var authSigninKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
            );
            var token = new JwtSecurityToken(
              issuer: _configuration["JWT:ValidIssuer"],
              audience: _configuration["JWT:ValidAudience"],
              expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:Expires"])),
              claims: authclaims,
              signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)  
            );

            return token;
        }
    }
}