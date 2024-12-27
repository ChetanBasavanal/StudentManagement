using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();

        Task<Course?> GetByIdAsync(int id);

        Task<Course> CreateAsync(Course course);

        Task<Course?> UpdateAsync(int id,Course course);
        Task<Course?> DeleteAsync(int id);
    }
}
