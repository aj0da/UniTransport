using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.RequestedTripVM
{
	public class RequestNowVM
	{
		[Required(ErrorMessage = "Departure Location is required")]
		public string DepartureLocation { get; set; }

        [Required(ErrorMessage = "Arrival Location is required")]
        public string ArrivalLocation { get; set; }
		[Required(ErrorMessage = "Departure Date is required")]
		[DataType(DataType.Date, ErrorMessage ="Only date type is accept")]
		public DateTime DepartureDate { get; set; }
        [Required(ErrorMessage = "Departure Time is required")]
        [DataType(DataType.Time, ErrorMessage = "Only t time type is accept")]
        public DateTime DepartureTime { get; set; }
		public bool IsPrivateRide { get; set; } = false;
		[Required]
		public int StudentId { get; set; }
	}
}
