using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department>CreateAsync(Department department);

        Task<Department?> UpdateAsync(int Id,Department department);

        Task<Department?> DeleteAsync(int Id);
    }
}
