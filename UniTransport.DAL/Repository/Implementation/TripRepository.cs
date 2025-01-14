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
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        public TripRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Trip> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.Vehicle)
                .Include(t => t.RequestedTrip)
                .Include(t => t.Bookings)
                    .ThenInclude(b => b.Student)
                .FirstOrDefaultAsync(t => t.TripId == id);
        }

        public async Task<IEnumerable<Trip>> GetActiveTripsAsync()
        {
            return await _context.Trips
                .Include(t => t.Vehicle)
                .Where(t => t.TripStatus == TripStatus.Active && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetIncomingTripsAsync()
        {
            var tenMinutesFromNow = DateTime.Now.AddMinutes(10);

            return await _context.Trips
                .Include(t => t.Vehicle)
                .Where(t => t.TripStatus == TripStatus.Active
                        && !t.IsDeleted
                        && t.DepartureTime > tenMinutesFromNow)
                .OrderBy(t => t.DepartureTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId)
        {
            return await _context.Trips
                .Where(t => t.VehicleId == vehicleId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> UpdateTripStatusAsync(int tripId, TripStatus status)
        {
            var trip = await GetByIdAsync(tripId);
            if (trip == null) return false;

            try
            {
                trip.TripStatus = status;
                return await UpdateAsync(trip);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAvailableSeatsAsync(int tripId, int seats)
        {
            var trip = await GetByIdAsync(tripId);
            if (trip == null) return false;

            try
            {
                trip.AvailableSeats = seats;
                return await UpdateAsync(trip);
            }
            catch
            {
                return false;
            }
        }





        public async Task<List<Trip>> GetAvailableTripsForDateAsync(DateTime date)
        {
            return await _context.Trips
                .Where(t => t.DepartureTime.Date == date.Date
                        && t.AvailableSeats > 0
                        && t.TripStatus == TripStatus.Active)
                .OrderBy(t => t.DepartureTime)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetTripsByIdsAsync(List<string> tripIds)
        {
            return await _context.Trips
                .Where(t => tripIds.Contains(t.TripId.ToString()))
                .ToListAsync();
        }

        public async Task UpdateTripSeatsAsync(string tripId, int seatsToReduce)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip != null)
            {
                trip.AvailableSeats -= seatsToReduce;
                await _context.SaveChangesAsync();
            }
        }
    }
}
