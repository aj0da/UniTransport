using System.ComponentModel.DataAnnotations;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.TripVM
{
    public class CreateTripVM
    {
        [Required]
        public string DepartureLocation { get; set; }

        [Required]
        public string ArrivalLocation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AvailableSeats { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public TripStatus TripStatus { get; set; } = TripStatus.Active;

        // Optional: Link to a requested trip
        public int? RequestedTripId { get; set; }
    }
}
