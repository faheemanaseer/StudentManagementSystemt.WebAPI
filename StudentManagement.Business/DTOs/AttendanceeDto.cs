using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.DTOs
{
    public class AttendanceeDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int CourseId { get; set; }   
        public bool IsPresent { get; set; } 
        public DateTime Date {  get; set; }

    }
}
