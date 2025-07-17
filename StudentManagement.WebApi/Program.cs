////using AutoMapper;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.EntityFrameworkCore.SqlServer;
////using StudentManagement.Business.Interfaces;
////using StudentManagement.Business.Mapping;
////using StudentManagement.Business.Services;
////using StudentManagement.DataAccess.Data;
////using StudentManagement.DataAccess.Interfaces;
////using StudentManagement.DataAccess.Repositories;

////var builder = WebApplication.CreateBuilder(args);
////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
////builder.Services.AddAutoMapper(typeof(MappingProfile));
////// Add services to the container.
////// Register services and repositories
////builder.Services.AddScoped<IUserService, UserService>();
////builder.Services.AddScoped<IStudentService, StudentService>();
////builder.Services.AddScoped<IStudentRepository, StudentRepository>();

////builder.Services.AddScoped<ICourseService, CourseService>();
////builder.Services.AddScoped<ICourseRepository, CourseRepository>();

////builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
////builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();

////builder.Services.AddScoped<IAttendanceeRepository, AttendanceeRepository>();
////builder.Services.AddScoped<IAttendanceeService, AttendanceeService>();

////builder.Services.AddScoped<IInstructorRepository, InstructorRepository>(); // ✅ This is what fixes the error
////builder.Services.AddScoped<IInstructorService, InstructorService>();


////builder.Services.AddControllers();
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

////app.Run();



//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;
//using StudentManagement.DataAccess.Data;
//using StudentManagement.Business.Interfaces;
//using StudentManagement.Business.Services;
//using StudentManagement.DataAccess.Interfaces;
//using StudentManagement.DataAccess.Repositories;
//using Microsoft.EntityFrameworkCore;
//using AutoMapper;
//using StudentManagement.Business.Mapping;
//using StudentManagement.Application.Entities;

//var builder = WebApplication.CreateBuilder(args);

//// Database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// AutoMapper
//builder.Services.AddAutoMapper(typeof(MappingProfile));

//// Custom Services and Repos
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IStudentService, StudentService>();
//builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//builder.Services.AddScoped<ICourseService, CourseService>();
//builder.Services.AddScoped<ICourseRepository, CourseRepository>();
//builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
//builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();
//builder.Services.AddScoped<IAttendanceeRepository, AttendanceeRepository>();
//builder.Services.AddScoped<IAttendanceeService, AttendanceeService>();
//builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
//builder.Services.AddScoped<IInstructorService, InstructorService>();

//// JWT Settings
//var jwtKey = builder.Configuration["Jwt:Key"];
//var jwtIssuer = builder.Configuration["Jwt:Issuer"];

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = jwtIssuer,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//    };
//});

//builder.Services.AddAuthorization();

//builder.Services.AddControllers();

//// Swagger + JWT Auth in Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(opt =>
//{
//    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Enter JWT token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });
//    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme {
//                Reference = new OpenApiReference {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// SuperAdmin Seeder
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//    // ✅ 1. Seed Roles first
//    if (!db.Roles.Any())
//    {
//        db.Roles.AddRange(
//            new Role { Id = 1, Name = "SuperAdmin" },
//            new Role { Id = 2, Name = "Admin" },
//            new Role { Id = 3, Name = "Student" }
//        );
//        db.SaveChanges();
//    }

//    // ✅ 2. Seed SuperAdmin user
//    if (!db.Users.Any(u => u.Email == "superadmin@example.com"))
//    {
//        db.Users.Add(new User
//        {
//            Name = "Super Admin",
//            Email = "superadmin@example.com",
//            Password = "1234", // ❗️Hash in real apps
//            RoleId = 1 // now this exists
//        });
//        db.SaveChanges();
//    }
//}


//app.UseHttpsRedirection();
//app.UseAuthentication(); // Add before authorization
//app.UseAuthorization();

//app.MapControllers();

//app.Run();




using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagement.Application.Entities;
using StudentManagement.Business.Interfaces;
using StudentManagement.Business.Mapping;
using StudentManagement.Business.Services;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Interfaces;
using StudentManagement.DataAccess.Repositories;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();
builder.Services.AddScoped<IAttendanceeRepository, AttendanceeRepository>();
builder.Services.AddScoped<IAttendanceeService, AttendanceeService>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IInstructorService, InstructorService>();


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

       
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddAuthorization();

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    
    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { Name = "SuperAdmin" },
            new Role { Name = "Admin" },
            new Role { Name = "Student" }
        );
        db.SaveChanges();
    }

    
    var superAdminRole = db.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");

    if (superAdminRole != null && !db.Users.Any(u => u.Email == "superadmin@example.com"))
    {
        db.Users.Add(new User
        {
            Name = "Super Admin",
            Email = "superadmin@example.com",
            Password = "1234", 
            RoleId = superAdminRole.Id
        });
        db.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
