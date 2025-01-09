using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public VehicleType VehicleType { get; set; } = VehicleType.Microbus;
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Trip> Trips { get; set; }
    }
}
