using PersonsApp.Dtos.Auth;
using PersonsApp.Dtos.Common;

namespace PersonsApp.Services.Auth
{
    public interface IAuthService
    {
       Task<ResponseDto<LoginResponseDto>>LoginAsync(LoginDto dto);
       Task<ResponseDto<LoginResponseDto>>RefreshTokenAsync(RefreshTokenDto dto);

    }
}