using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.DAL.Repository.Implementation
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Booking> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Student)
                    .ThenInclude(s => s.User)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStudentIdAsync(int studentId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                .Where(b => b.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTripIdAsync(int tripId)
        {
            return await _context.Bookings
                .Include(b => b.Student)
                    .ThenInclude(s => s.User)
                .Where(b => b.TripId == tripId)
                .ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await GetByIdAsync(bookingId);
            if (booking == null) return false;

            try
            {
                booking.IsCancelled = true;
                return await UpdateAsync(booking);
            }
            catch
            {
                return false;
            }
        }
    }

}
