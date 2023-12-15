using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace NC6.Models
{
    [Table("Groups")]
    public class Group
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string? Name { get; set; }

        [Display(Name = "Grupa")]
        public string? Sname { get; set; }

        public virtual ICollection<Student>? Students { get; }

    }
}

