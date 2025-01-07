using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.DAL.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;

        // Navigation properties
        public Student Student { get; set; }
        public Trip Trip { get; set; }
    }
}
