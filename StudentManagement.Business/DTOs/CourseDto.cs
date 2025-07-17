using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.DTOs
{
    public class CourseDto
    {
        public int SId { get; set; }
        public string Title { get; set; }
        public int? InstructorId { get; set; }
        public string InstructorName { get; set; }
    }
}
