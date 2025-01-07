using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Entities
{
    public class RequestedTrip
    {
        [Key]
        public int RequestedTripId { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public RequestedTripStatus Status { get; set; } = RequestedTripStatus.Pending;

        // Navigation properties
        [Required]
        public Student Student { get; set; }
    }
}
