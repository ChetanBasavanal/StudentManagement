using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee> GetByIDAsync(int ID);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(int ID, Employee employee);
        Task<Employee> DeleteAsync(int ID);
    }
}
