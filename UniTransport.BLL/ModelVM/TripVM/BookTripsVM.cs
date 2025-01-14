using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.TripVM
{
    public class BookTripsVM
    {
        public int TripId { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public double Price { get; set; }
        public bool Isbooked { get; set; } = false; //To handle booking 
        public string LicensePlate { get; set; }  

        //public ICollection<Booking> Bookings { get; set; }
    }
}
