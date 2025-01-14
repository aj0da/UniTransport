using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.RequestedTripVM;

namespace UniTransport.BLL.Service.Interface
{
    public interface IRequestedTripService
    {
        bool CreatRequestedTrip(RequestNowVM requestNowVM, string userId);
    }
}
