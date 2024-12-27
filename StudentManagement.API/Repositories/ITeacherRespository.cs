using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface ITeacherRespository
    {
        Task<List<Teacher>> GetAllAsync();

        Task<Teacher?> GetByIdAsync(int id);

        Task<Teacher> CreateAsync(Teacher teacher);

        Task<Teacher?> UpdateAsync(int Id, Teacher teacher);

        Task<Teacher?> DeleteAsync(int Id);
    }
}
