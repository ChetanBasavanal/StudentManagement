using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Data
{
    public class StudentManagementDbContext:DbContext
    {
        public StudentManagementDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
