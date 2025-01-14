using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByStudentIdAsync(int studentId);
        Task<IEnumerable<Booking>> GetBookingsByTripIdAsync(int tripId);
        Task<bool> CancelBookingAsync(int bookingId);


        Task<List<Booking>> GetUserBookingsForDateAsync(string userId, DateTime date);
        Task<bool> CreateBookingsAsync(List<Booking> bookings);
    }
}
