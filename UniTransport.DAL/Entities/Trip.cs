using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.DAL.Entities
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public Vehicle Vehicle { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        
        public Trip()
        {
            AvailableSeats = Vehicle.Capacity;
        }
    }
}
