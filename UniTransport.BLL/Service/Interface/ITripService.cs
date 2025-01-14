using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;

namespace UniTransport.BLL.Service.Interface
{
    public interface ITripService
    {
        IEnumerable<BookTripsVM> GetIncomingTrips();
        bool BookTrip(IEnumerable<BookTripsVM> trips, string userId);
	}
}
