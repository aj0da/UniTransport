using UniTransport.DAL.Entities;

namespace UniTransport.BLL.Service.Interface
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(int id);
        Task<bool> AddVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);
    }
}
