using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class Course
    {
        [Key]
        public int SId { get; set; }
        [Required]
        public string Title { get; set; }
       [ValidateNever]
        public ICollection<StudentCourse> StudentCourses { get; set; }
        public int? InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}
