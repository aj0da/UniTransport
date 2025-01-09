using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.DAL.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public bool IsCancelled { get; set; } = false;
        public int StudentId { get; set; }
        public int TripId { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Trip Trip { get; set; }
    }
}
