using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.ModelVM.RequestedTripVM
{
	public class RequestedTripStudentProfileVM
	{
		public int RequestedTripId { get; set; }
		public string DepartureLocation { get; set; }
		public string ArrivalLocation { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime RequestTime { get; set; }
		public bool IsPrivateRide { get; set; }
		public RequestedTripStatus Status { get; set; }
	}
}
