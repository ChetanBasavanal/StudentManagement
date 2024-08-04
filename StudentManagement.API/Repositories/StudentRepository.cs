using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Models.DomainModels;

namespace StudentManagement.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentManagementDbContext studentManagementDbContext;

        public StudentRepository(StudentManagementDbContext studentManagementDbContext)
        {
            this.studentManagementDbContext = studentManagementDbContext;
        }

        public async Task<Student> CreateAsync(Student studentDomainModel)
        {
            await studentManagementDbContext.Students.AddAsync(studentDomainModel);
            await studentManagementDbContext.SaveChangesAsync();
            return studentDomainModel;
        }

        public async Task<Student?> DeleteAsync(int id)
        {
            var StudentDomainModel= await studentManagementDbContext.Students.FirstOrDefaultAsync(x=>x.StudentID==id);  

            if (StudentDomainModel != null)
            {
                studentManagementDbContext.Students.Remove(StudentDomainModel);
                await studentManagementDbContext.SaveChangesAsync();
                return StudentDomainModel;
            }
            return null;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await studentManagementDbContext.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await studentManagementDbContext.Students.FirstOrDefaultAsync(x=>x.StudentID==id);

        }

        public async Task<Student?> UpdateAsync(int id, Student studentDomainModel)
        {
            var StudentDomain = await studentManagementDbContext.Students.FirstOrDefaultAsync(x=>x.StudentID==id);
            if (StudentDomain == null)
            {
                return null;
            }
            //UPdate data in DB from Domainmodel

            StudentDomain.StudentID=studentDomainModel.StudentID;
            StudentDomain.FirstName=studentDomainModel.FirstName;
            StudentDomain.LastName=studentDomainModel.LastName;
            StudentDomain.DOB=studentDomainModel.DOB;
            StudentDomain.Address=studentDomainModel.Address;
            StudentDomain.Email=studentDomainModel.Email;

            await studentManagementDbContext.SaveChangesAsync();

            return StudentDomain;
        }
    }
}
