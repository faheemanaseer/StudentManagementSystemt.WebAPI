using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
