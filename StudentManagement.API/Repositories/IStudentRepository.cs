using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student?>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student> CreateAsync(Student studentDomainModel);

        Task<Student?> UpdateAsync(int id, Student studentDomainModel);

        Task<Student?> DeleteAsync(int id);
    }
}
