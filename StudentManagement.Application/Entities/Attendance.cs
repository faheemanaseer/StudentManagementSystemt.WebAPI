using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Entities
{
    public class Attendance
    {
        [Key]
        public int AttenId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }

}
