using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PersonsApp.Entities
{
    public class UserEntity : IdentityUser
    {
        [Column("first_name")]
        [Required]
        public string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        public string LastName { get; set; }

        [Column("avatar_url")]
        public string AvatarUrl { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("refresh_token_expiry")]
        public DateTime RefreshTokenExpiry { get; set; }
    }
}