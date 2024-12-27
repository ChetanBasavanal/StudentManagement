using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public EmployeeRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        async Task<Employee> IEmployeeRepository.CreateAsync(Employee employee)
        {
            await studentManagementDbContext.Employees.AddAsync(employee);
            await studentManagementDbContext.SaveChangesAsync();
            return employee;
        }

        async Task<Employee?> IEmployeeRepository.DeleteAsync(int ID)
        {
            var EmployeeDomainModel = await studentManagementDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == ID);
            
            if(EmployeeDomainModel != null)
            {
                studentManagementDbContext.Remove(EmployeeDomainModel);
                await studentManagementDbContext.SaveChangesAsync();
                return EmployeeDomainModel;
            }
            return null;
        }

        async Task<List<Employee>> IEmployeeRepository.GetAllAsync()
        {
            return await studentManagementDbContext.Employees.ToListAsync();    
        }

        async Task<Employee?> IEmployeeRepository.GetByIDAsync(int ID)
        {
            return await studentManagementDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == ID);
        }

        async Task<Employee?> IEmployeeRepository.UpdateAsync(int ID, Employee employee)
        {
            var EmployeeDomainModel = await studentManagementDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == ID);

            if(EmployeeDomainModel != null)
            {
                EmployeeDomainModel.EmployeeID = employee.EmployeeID;
                EmployeeDomainModel.EmployeeName = employee.EmployeeName;
                EmployeeDomainModel.Department= employee.Department;
                EmployeeDomainModel.DepartmentID = employee.DepartmentID;

                await studentManagementDbContext.SaveChangesAsync();
                return EmployeeDomainModel;
            }
            return null;
        }
    }
}
