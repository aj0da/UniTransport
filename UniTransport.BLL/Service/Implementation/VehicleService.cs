using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> AddVehicleAsync(Vehicle vehicle)
        {
            return await _vehicleRepository.CreateAsync(vehicle);
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            return await _vehicleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Where(v => v.IsActive);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicle)
        {
            return await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}
