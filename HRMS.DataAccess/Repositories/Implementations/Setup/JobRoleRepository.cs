using HRMS.DataAccess.Context;
using HRMS.DataAccess.Repositories.Interfaces.Setup;
using HRMS.Models.Entities.Setup;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repositories.Implementations.Setup
{
    public class JobRoleRepository : IJobRoleRepository
    {
        private readonly HRMSDbContext _context;

        public JobRoleRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobRole>> GetsAsync()
        {
            return await _context.JobRoles
                                   .Include(d => d.Department) //
                                   .ToListAsync();
        }
        public async Task<JobRole?> GetByTitleAsync(string title)
        {
            return await _context.JobRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Title == title);
        }

        public async Task<JobRole> GetByIdAsync(int id)
        {
            return await _context.JobRoles.FindAsync(id);
        }

        public async Task<JobRole?> GetByTitleAndDepartmentAsync(string title, int departmentId)
        {
            return await _context.JobRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(jr => jr.Title == title && jr.DepartmentId == departmentId);
        }
        public async Task<JobRole> AddAsync(JobRole entity)
        {
            entity = (await _context.JobRoles.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<JobRole> UpdateAsync(JobRole entity)
        {
            entity = _context.JobRoles.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobRole?> DeleteAsync(int id)
        {
            var jobRole = await _context.JobRoles.FindAsync(id);
            if (jobRole != null)
            {
                _context.JobRoles.Remove(jobRole);
                await _context.SaveChangesAsync();
                return jobRole;
            }
            return null;
        }
    }
}
