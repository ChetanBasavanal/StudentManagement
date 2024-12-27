using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class TeacherRepository : ITeacherRespository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public TeacherRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            await studentManagementDbContext.Teachers.AddAsync(teacher);
            await studentManagementDbContext.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher?> DeleteAsync(int Id)
        {
            var DeletedResult= await studentManagementDbContext.Teachers.FirstOrDefaultAsync(x=>x.TeacherID==Id);

            if (DeletedResult != null)
            {
                studentManagementDbContext.Teachers.Remove(DeletedResult);
                await studentManagementDbContext.SaveChangesAsync();
                return DeletedResult;
            }
            return null;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await studentManagementDbContext.Teachers.Include("Department").ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await studentManagementDbContext.Teachers.Include("Department").FirstOrDefaultAsync(x=>x.TeacherID == id);
        }

        public async Task<Teacher?> UpdateAsync(int Id, Teacher teacher)
        {
            var DomainData= await studentManagementDbContext.Teachers.FirstOrDefaultAsync(x=>x.TeacherID==Id);

            if (DomainData!=null)
            {
                DomainData.FirstName = teacher.FirstName;
                DomainData.LastName = teacher.LastName;
                DomainData.Subject = teacher.Subject;
                DomainData.DepartmentID = teacher.DepartmentID;
                await studentManagementDbContext.SaveChangesAsync();
                return DomainData;
            }
            return null;
        }
    }
}
