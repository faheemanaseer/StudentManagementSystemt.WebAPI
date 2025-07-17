using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.DTOs
{
    public class CourseInstructorDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string InstructorName { get; set; } = "Not Assigned";
    }

}
