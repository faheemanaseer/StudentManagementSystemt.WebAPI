using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class Student
    {
        [Key]
        public int UId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, Range(1, 120, ErrorMessage = "Enter valid age")]
        public int Age { get; set; }

        [Required(ErrorMessage ="Student Card Image is Required")]
        public string? CardImagePath { get; set; }
        [ValidateNever]
        public ICollection<StudentCourse> StudentCourses { get; set; }
       
        public int? UserId { get; set; }
        public User User { get; set; }

    }
}
