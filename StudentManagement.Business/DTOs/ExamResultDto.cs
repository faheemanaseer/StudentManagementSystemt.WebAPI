using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.DTOs
{
    public class CreateExamResultDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        [Range(0, 100)]
        public double Marks { get; set; }
    }


    public class ExamResultDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public double Marks { get; set; }
        public string Grade { get; set; }
    }
}
