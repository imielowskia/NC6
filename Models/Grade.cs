using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NC6.Models
{
    [Table("Grades")]    
    public class Grade
    {
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

        [Display(Name ="Student")]
        public Student? Student { get; set; }

        [Display(Name ="Przedmiot")]
        public Course? Course { get; set; }

        [Display(Name ="Ocena")]
        [Precision(1,1)]
        public double grade { get; set; }
    }
}
