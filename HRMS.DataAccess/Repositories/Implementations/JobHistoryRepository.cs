using HRMS.DataAccess.Context;
using HRMS.DataAccess.Repositories.Interfaces;
using HRMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repositories.Implementations
{
    public class JobHistoryRepository : IJobHistoryRepository
    {
        private readonly HRMSDbContext _context;

        public JobHistoryRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobHistory>> GetsAsync()
        {
            return await _context.JobHistories
                                 .Include(jh => jh.Employee)
                                 .Include(jh => jh.Manager)
                                 .Include(jh => jh.JobRole)
                                 .ToListAsync();
        }

        public async Task<JobHistory> GetByIdAsync(int id)
        {
            return await _context.JobHistories
                                 .Include(jh => jh.Employee)
                                 .Include(jh => jh.Manager)
                                 .Include(jh => jh.JobRole)
                                 .FirstOrDefaultAsync(jh => jh.Id == id);
        }

        public async Task<JobHistory> AddAsync(JobHistory entity)
        {
            entity = (await _context.JobHistories.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobHistory> UpdateAsync(JobHistory entity)
        {
            var existingEntity = await _context.JobHistories.FindAsync(entity.Id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException("JobHistory not found.");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        public async Task<JobHistory?> DeleteAsync(int id)
        {
            var jobHistory = await _context.JobHistories.FindAsync(id);
            if (jobHistory != null)
            {
                _context.JobHistories.Remove(jobHistory);
                await _context.SaveChangesAsync();
                return jobHistory;
            }
            return null;
        }

        public async Task<JobHistory> GetByEmployeeAndManagerAsync(int employeeId, int? managerId, int currentJobHistoryId)
        {
            return await _context.JobHistories
                    .FirstOrDefaultAsync(jh =>
                    jh.EmployeeId == employeeId &&
                    jh.ManagerId == managerId &&
                    jh.Id != currentJobHistoryId); // Skip the current edited record
        }

    }
}