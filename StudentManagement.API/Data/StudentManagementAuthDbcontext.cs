using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.Data
{
    public class StudentManagementAuthDbcontext : IdentityDbContext
    {
        public StudentManagementAuthDbcontext(DbContextOptions<StudentManagementAuthDbcontext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ReaderRoleID = "3cac33dd-1d9c-4b33-a0bb-2dab5f786b8b";
            var WriterRoleID = "7d1812b1-f675-4635-88db-0568617e0267";

            var Roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = ReaderRoleID,
                    ConcurrencyStamp = ReaderRoleID,
                    Name="Reader",
                    NormalizedName= "Reader".ToUpper(),
                },

                new IdentityRole
                {
                    Id=WriterRoleID,
                    ConcurrencyStamp = WriterRoleID,
                    Name="Writer",
                    NormalizedName= "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(Roles);
        }
    }
}
