using HRMS.DataAccess.Context;
using HRMS.DataAccess.Repositories.Interfaces.Setup;
using HRMS.Models.Entities.Setup;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repositories.Implementations.Setup
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRMSDbContext _context;

        public DepartmentRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetsAsync()
        {
            return await _context.Departments
                                   .Include(d => d.Location) //
                                   .ToListAsync();

        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }

        public async Task<Department> AddAsync(Department entity)
        {
            entity = (await _context.Departments.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            entity = _context.Departments.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Department> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return department;
            }
            return null;
        }
    }
}
