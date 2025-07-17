using StudentManagement.Application.Entities;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Interfaces;
using StudentManagement.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Services
{
    public class AttendanceeService : IAttendanceeService
    {
        private readonly IAttendanceeRepository _attendanceeRepository;
        private readonly IStudentRepository _studentRepository;
        public AttendanceeService(IAttendanceeRepository attendanceeRepository, IStudentRepository studentRepository)
        {
            _attendanceeRepository = attendanceeRepository;
            _studentRepository = studentRepository;
        }
        public async Task<List<AttendanceeDto>> GetAttendanceAsync(int courseId, DateTime date)
        {
            var existing = await _attendanceeRepository.GetByCourseAndDateAsync(courseId, date);
            if (existing.Any())
            {
                return existing.Select(a => new AttendanceeDto
                {
                    StudentId = a.StudentId,
                    StudentName = a.Student.Name,
                    CourseId = courseId,
                    Date = date,
                    IsPresent = a.IsPresent
                }).ToList();
            }


            var students = await _studentRepository.GetStudentsByCourseId(courseId);
            return students.Select(s => new AttendanceeDto
            {
                StudentId = s.UId,
                StudentName = s.Name,
                CourseId = courseId,
                Date = date,
                IsPresent = false
            }).ToList();
        }

        public async Task SaveAttendanceAsync(List<AttendanceeDto> attendanceList)
        {
            foreach (var dto in attendanceList)
            {
                var existing = await _attendanceeRepository
                .GetByCourseAndDateAndStudentAsync(dto.CourseId, dto.Date, dto.StudentId);

                if (existing == null)
                {

                    var attendance = new Attendance
                    {
                        StudentId = dto.StudentId,
                        CourseId = dto.CourseId,
                        Date = dto.Date,
                        IsPresent = dto.IsPresent
                    };

                    await _attendanceeRepository.AddAsync(attendance);
                }
                else
                {

                    existing.IsPresent = dto.IsPresent;
                    await _attendanceeRepository.UpdateAsync(existing);
                }
            }
        }
        public async Task DeleteExistingRecordsAsync(int courseId, DateTime date)
        {
            await _attendanceeRepository.DeleteByCourseAndDateAsync(courseId, date);
        }




    }
}
