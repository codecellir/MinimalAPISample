using Microsoft.EntityFrameworkCore;
using MinimalAPISample.Data;
using MinimalAPISample.Entities;

namespace MinimalAPISample.Repositories
{
    public interface IStudentRepository
    {
        Task CreateAsync(Student student);
        Task EditAsync(Student student);
        Task DeleteAsync(int studentId);
        Task<Student> GetAsync(int studentId);
        Task<IEnumerable<Student>> GetAllAsync();
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Student student)
        {
            student.Id = 0;
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int studentId)
        {
            var student = await GetAsync(studentId);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllAsync() => await _context.Students.AsNoTracking().ToListAsync();

        public async Task<Student> GetAsync(int studentId)
        {
            var student = await _context.Students.AsNoTracking().SingleOrDefaultAsync(d => d.Id == studentId);
            return student;
        }
    }
}
