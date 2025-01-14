using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.BookingVM;
using UniTransport.BLL.ModelVM.RequestedTripVM;
using UniTransport.DAL.Entities;

namespace UniTransport.BLL.ModelVM.StudentVM
{
	public class StudentProfileVM
	{
		public int StudentId { get; set; }
		public int UniversityStudentId { get; set; }

		public string StudentName { get; set; }
		public string? Image { get; set; }
		public string StudentUserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public ICollection<BookingStudentProfileVM> Bookings { get; set; }
		public ICollection<RequestedTripStudentProfileVM> RequestedTrips { get; set; }
	}
}
