using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.BLL.ModelVM.BookingVM
{
    public class BookingStatisticsVM
    {
        public int TotalBookings { get; set; }
        public int ActiveBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalPassengers { get; set; }
        public Dictionary<string, int> BookingsByRoute { get; set; } = new();
        public Dictionary<string, int> BookingsByVehicle { get; set; } = new();
        public Dictionary<DateTime, int> BookingsByDate { get; set; } = new();
    }
}
