using System.ComponentModel.DataAnnotations;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.TripVM
{
    public class EditTripVM
    {
        [Required]
        public int TripId { get; set; }

        [Required]
        public string DepartureLocation { get; set; } = string.Empty;

        [Required]
        public string ArrivalLocation { get; set; } = string.Empty;

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

        public TripStatus TripStatus { get; set; }
    }
}
