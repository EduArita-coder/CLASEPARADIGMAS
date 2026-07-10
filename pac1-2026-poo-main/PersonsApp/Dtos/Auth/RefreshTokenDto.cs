using System.ComponentModel.DataAnnotations;

namespace PersonsApp.Dtos.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}