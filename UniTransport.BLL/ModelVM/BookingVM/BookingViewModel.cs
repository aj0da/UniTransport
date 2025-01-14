using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;

namespace UniTransport.BLL.ModelVM.BookingVM
{
    public class BookingViewModel
    {
        public DateTime BookingDate { get; set; }
        public List<TripViewModel> AvailableTrips { get; set; }
        public List<string> SelectedTripIds { get; set; }
    }
}
