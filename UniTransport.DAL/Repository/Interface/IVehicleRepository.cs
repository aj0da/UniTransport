using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Vehicle> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType type);
        Task<bool> ToggleVehicleStatusAsync(int vehicleId);
    }
}
