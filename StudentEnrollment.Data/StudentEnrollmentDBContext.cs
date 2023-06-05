using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEnrollment.Data
{
    public class StudentEnrollmentDBContext : IdentityDbContext
    {
        public StudentEnrollmentDBContext(DbContextOptions<StudentEnrollmentDBContext> options) : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }   
        public DbSet<Enrollment> Enrollments { get; set; } 
    }
}
