using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class ExamResult
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public double Marks { get; set; }

        public string Grade { get; set; }

        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
