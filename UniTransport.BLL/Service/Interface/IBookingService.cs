using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;

namespace UniTransport.BLL.Service.Interface
{
    public interface IBookingService
    {
        Task<List<TripViewModel>> GetAvailableTripsAsync(DateTime date);
        Task<bool> CreateBookingAsync(string userId, List<string> tripIds);
        Task<decimal> CalculateTotalPriceAsync(List<string> tripIds);
        Task<bool> ValidateBookingAsync(string userId, List<string> tripIds);
    }
}
