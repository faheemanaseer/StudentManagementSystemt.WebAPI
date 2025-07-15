using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class StudentCourse
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
