using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public CourseRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        public async Task<Course> CreateAsync(Course course)
        {
           await studentManagementDbContext.Courses.AddAsync(course);
           await studentManagementDbContext.SaveChangesAsync();
           return course;
        }

        public async Task<Course?> DeleteAsync(int id)
        {
              var CourseData= await studentManagementDbContext.Courses.FirstOrDefaultAsync(x=>x.CourseID==id);
            if (CourseData != null)
            {
                studentManagementDbContext.Courses.Remove(CourseData);
                await studentManagementDbContext.SaveChangesAsync();
                return CourseData;
            }
            return null;
        }

        public async Task<List<Course>> GetAllAsync()
        {
           return await studentManagementDbContext.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await studentManagementDbContext.Courses.FirstOrDefaultAsync(x=>x.CourseID== id);
            
        }

        public async Task<Course?> UpdateAsync(int id, Course course)
        {
            var CourseData= await studentManagementDbContext.Courses.FirstOrDefaultAsync(x => x.CourseID == id);
            if (CourseData != null)
            {
                
                CourseData.CourseName = course.CourseName;
                CourseData.Description = course.Description;
                CourseData.Credits = course.Credits;
                await studentManagementDbContext.SaveChangesAsync();
                return CourseData;
            }
            return null;
        }
    }
}
