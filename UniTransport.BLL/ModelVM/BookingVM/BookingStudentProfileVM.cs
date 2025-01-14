using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.BookingVM
{
	public class BookingStudentProfileVM
	{
		public int BookingId { get; set; }
		public DateTime BookingTime { get; set; }
		public bool IsCancelled { get; set; } = false;
		public int TripId { get; set; }

		public string DepartureLocation { get; set; }
		public string ArrivalLocation { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public double Price { get; set; }
		public TripStatus TripStatus { get; set; }
		
		public string LicensePlate { get; set; }
	}
}
