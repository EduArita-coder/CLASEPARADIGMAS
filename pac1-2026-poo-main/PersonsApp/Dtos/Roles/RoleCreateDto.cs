using System.ComponentModel.DataAnnotations;

namespace PersonsApp.Dtos.Roles
{
    public class RoleCreateDto
    {
        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}