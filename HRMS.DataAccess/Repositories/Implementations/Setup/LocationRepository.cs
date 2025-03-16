using HRMS.DataAccess.Context;
using HRMS.DataAccess.Repositories.Interfaces.Setup;
using HRMS.Models.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace HRMS.DataAccess.Repositories.Implementations.Setup
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HRMSDbContext _context;

        public LocationRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<Location> GetByIdAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task<Location> GetByNameAsync(string name)
        {
            return await _context.Locations
                            .FirstOrDefaultAsync(l => l.Name == name); // ✅ Use LINQ instead of Find()
        }

        public async Task<Location> AddAsync(Location entity)
        {
            entity = (await _context.Locations.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Location> UpdateAsync(Location entity)
        {
            var existingEntity = await _context.Locations.FindAsync(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Locations.Update(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Location?> DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return location;
            }
            return null;
        }
    }
}
