using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace NC6.Models
{
    [Table("Attendances")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataS { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId {  get; set; }

        [Display(Name ="Przedmiot")]
        public Course Course { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [Display(Name ="Student")]
        public Student Student { get; set; }
    }
}
