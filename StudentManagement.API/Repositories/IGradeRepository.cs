using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public interface IGradeRepository
    {
        Task<List<Grade>> GetAllAsync(); 

        Task<Grade?> GetByIdAsync(int id);

        Task<Grade> CreateAsync(Grade grade);

        Task<Grade?> UpdateAsync(int Id, Grade grade);

        Task<Grade?> DeleteAsync(int id);
    }
}
