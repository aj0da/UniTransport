using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.TripVM
{
    public class TripViewModel
    {
        public int? TripId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartureLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string ArrivalLocation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        public TripStatus TripStatus { get; set; }
    }
}
