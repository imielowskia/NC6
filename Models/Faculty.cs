using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NC6.Models
{
    [Table("Faculties")]
    public class Faculty
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Kierunek")]
        public string? Name { get; set; }

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Display(Name = "Grupy")]
        public virtual ICollection<Group>? Groups { get; set; }

        [Display(Name = "Przedmioty")]
        public virtual ICollection<Course>? Courses { get; set; }
    }
}
