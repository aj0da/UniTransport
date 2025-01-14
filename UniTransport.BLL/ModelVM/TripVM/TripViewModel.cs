using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.BLL.ModelVM.TripVM
{
    public class TripViewModel
    {
        public string CarNumber { get; set; }
        public string Destination { get; set; }
        public int AvailableSeats { get; set; }
        public string TripTime { get; set; }
        public decimal Price { get; set; }
        public DateTime TripDate { get; set; }
    }
}
