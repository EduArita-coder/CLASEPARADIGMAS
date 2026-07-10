using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonsApp.Entities
{
    [Table("countries")]
    public class CountryEntity : BaseEntity
    {
        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("alpha_code_3")]
        [Required]
        public string AlphaCode3 { get; set; }

        public virtual IEnumerable<PersonEntity> Persons { get; set; }
    }
}