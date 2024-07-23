using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public DepartmentRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        public async Task<Department> CreateAsync(Department department)
        {
            await studentManagementDbContext.Departments.AddAsync(department);
            await studentManagementDbContext.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> DeleteAsync(int Id)
        {
            var DomainModelData= await studentManagementDbContext.Departments.FirstOrDefaultAsync(x=>x.DepartmentID==Id);

            if (DomainModelData != null)
            {
                studentManagementDbContext.Departments.Remove(DomainModelData);
                await studentManagementDbContext.SaveChangesAsync();
                return DomainModelData;
            }
            return null;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            //Get data from Domain model
            return await studentManagementDbContext.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await studentManagementDbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentID == id);
        }

        public async Task<Department?> UpdateAsync(int Id, Department department)
        {
            var DepartmentData= await studentManagementDbContext.Departments.FirstOrDefaultAsync(x=>x.DepartmentID == Id);
            if (DepartmentData == null)
                return null;
            //update data from Domain model to DB
            DepartmentData.DepartmentName = department.DepartmentName;
            DepartmentData.Head = department.Head;
            await studentManagementDbContext.SaveChangesAsync();

            return DepartmentData;
        }
    }
}
