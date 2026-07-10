using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PersonsApp.Entities
{
    public class RoleEntity : IdentityRole
    {
        [Column("description")]
        public string Description { get; set; }
    }
}