using AutoMapper;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.EnrolledCourses,
                    opt => opt.MapFrom(src => src.StudentCourses.Select(sc => sc.Course)));

            
            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.UId, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());

            
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name));

            CreateMap<CourseDto, Course>();

       
            CreateMap<Instructor, InstructorDto>().ReverseMap();

          
            CreateMap<StudentCourse, StudentCourse>().ReverseMap();


            CreateMap<CreateExamResultDto, ExamResult>().ReverseMap();
            CreateMap<ExamResult, ExamResultDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Title))
                .ReverseMap();
        }
    }

}
