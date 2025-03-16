using HRMS.DataAccess.Context;
using HRMS.Models.Entities;
using HRMS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetsAsync()
        {
            return await _context.Employees
                                 .Include(e => e.JobHistories)
                                     .ThenInclude(jh => jh.JobRole)
                                 .Include(e => e.JobHistories)
                                     .ThenInclude(jh => jh.Manager)
                                 .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                                 .Include(e => e.JobHistories)
                                     .ThenInclude(jh => jh.JobRole)
                                 .Include(e => e.JobHistories)
                                     .ThenInclude(jh => jh.Manager)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee> GetByPhoneAsync(string phone)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Phone == phone);
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            entity = (await _context.Employees.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
            var existingEntity = await _context.Employees.FindAsync(entity.Id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        public async Task<Employee?> DeleteAsync(int id)
        {
            var jobHistories = _context.JobHistories.Where(jh => jh.ManagerId == id);
            if (jobHistories.Any())
            {
                _context.JobHistories.RemoveRange(jobHistories);
                await _context.SaveChangesAsync();
            }

            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }
    }
}
