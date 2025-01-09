using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Entities
{
    public class RequestedTrip
    {
        public int RequestedTripId { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public bool IsPrivateRide { get; set; } = false;
        public RequestedTripStatus Status { get; set; } = RequestedTripStatus.Pending;
        public int StudentId { get; set; }

        // Navigation properties       
        public Student Student { get; set; }
    }
}
