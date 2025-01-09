using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.DAL.Repository.Implementation
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Vehicle> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Trips)
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            return await _context.Vehicles
                .Where(v => v.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType type)
        {
            return await _context.Vehicles
                .Where(v => v.VehicleType == type && v.IsActive)
                .ToListAsync();
        }

        public async Task<bool> ToggleVehicleStatusAsync(int vehicleId)
        {
            var vehicle = await GetByIdAsync(vehicleId);
            if (vehicle == null) return false;

            try
            {
                vehicle.IsActive = !vehicle.IsActive;
                return await UpdateAsync(vehicle);
            }
            catch
            {
                return false;
            }
        }
    }
}
