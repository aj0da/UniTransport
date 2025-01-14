using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.BookingVM;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.DAL.Entities;

namespace UniTransport.BLL.Service.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByTripAsync(int tripId);
        Task<IEnumerable<Booking>> GetStudentBookingsAsync(int studentId);
        Task<bool> CancelBookingAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date);
        Task<BookingStatisticsVM> GetBookingStatisticsAsync();
        Task<List<TripViewModel>> GetAvailableTripsAsync(DateTime date);
        Task<bool> CreateBookingAsync(string userId, List<string> tripIds);
        Task<decimal> CalculateTotalPriceAsync(List<string> tripIds);
        Task<bool> ValidateBookingAsync(string userId, List<string> tripIds);
    }
}
