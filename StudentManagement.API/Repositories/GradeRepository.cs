using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public GradeRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        public async Task<Grade> CreateAsync(Grade grade)
        {
            await studentManagementDbContext.Grades.AddAsync(grade);
            await studentManagementDbContext.SaveChangesAsync();
            return grade;
        }

        public async Task<Grade?> DeleteAsync(int? Id)
        {
            var DomainData= await studentManagementDbContext.Grades.FirstOrDefaultAsync(x=>x.GradeID==Id);
            if (DomainData != null)
            {
                studentManagementDbContext.Grades.Remove(DomainData);
                await studentManagementDbContext.SaveChangesAsync();
                return DomainData;
            }
            return null;
        }

        public async Task<List<Grade>> GetAllAsync()
        {
            return await studentManagementDbContext.Grades.Include("Student").ToListAsync();
        }

        public async Task<Grade?> GetByIdAsync(int? id)
        {
           return await studentManagementDbContext.Grades.Include("Student").FirstOrDefaultAsync(x=>x.GradeID==id);

        }

        public async Task<Grade?> UpdateAsync(int? Id, Grade grade)
        {
            var DomainData= await studentManagementDbContext.Grades.FirstOrDefaultAsync(x=>x.GradeID == Id);

            if (DomainData != null)
            {
                DomainData.DateRecorded=grade.DateRecorded;
                DomainData.GradeGiven=grade.GradeGiven;
                DomainData.CourseName=grade.CourseName;
                DomainData.StudentID=grade.StudentID;
                await studentManagementDbContext.SaveChangesAsync();

                return DomainData;
            }
            return null;
        }
    }
}
