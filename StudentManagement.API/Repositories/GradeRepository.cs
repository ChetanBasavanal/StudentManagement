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

        public async Task<Grade> CreateAsync(Grade gradeDomainModel)
        {
           await studentManagementDbContext.Grades.AddAsync(gradeDomainModel);
           await studentManagementDbContext.SaveChangesAsync();
           return gradeDomainModel;
        }

        public async Task<Grade?> DeleteAsync(int id)
        {
            var GradeData= await studentManagementDbContext.Grades.FirstOrDefaultAsync(x=>x.GradeID==id);

            if (GradeData != null)
            {
                studentManagementDbContext.Grades.Remove(GradeData);
                await studentManagementDbContext.SaveChangesAsync();
                return GradeData;
            }
            return null;
        }

        public async Task<List<Grade>> GetAllAsync()
        {
            return await studentManagementDbContext.Grades.ToListAsync();
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await studentManagementDbContext.Grades.Include("Student").FirstOrDefaultAsync(x=>x.GradeID == id); 
        }

        public async Task<Grade?> UpdateAsync(int Id, Grade grade)
        {
            var GradeDTOData= await studentManagementDbContext.Grades.FirstOrDefaultAsync(x=>x.GradeID==Id);

            if(GradeDTOData!=null)
            {
                GradeDTOData.StudentID= grade.StudentID;
                GradeDTOData.GradeGiven= grade.GradeGiven;
                GradeDTOData.CourseName= grade.CourseName;
                GradeDTOData.DateRecorded=grade.DateRecorded;
                await studentManagementDbContext.SaveChangesAsync();

                return GradeDTOData;
            }
            return null;
        }
    }
}
