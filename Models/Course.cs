using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace NC6.Models
{
    [Table("Courses")]
    public class Course
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Przedmiot")]
        public string? Name { get; set; }

        [Display(Name = "Pkt. ECTS")]
        public int? ects { get; set; }

        [ForeignKey("FacultyId")]
        public int? FacultyId { get; set; }

        [Display(Name = "Kierunek")]
        public virtual Faculty? Faculty { get; set; }

        [Display(Name = "Grupy")]
        public virtual ICollection<Group>? Groups { get; set; }

        [Display(Name = "Oceny")]
        public ICollection<Grade>? Grades { get; set; }

        [Display(Name = "Obecność na zajęciach")]
        public ICollection<Attendance>? Attendances { get; set; }
    }
}

