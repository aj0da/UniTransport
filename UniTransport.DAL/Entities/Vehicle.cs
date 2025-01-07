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
        [Key]
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public VehicleType VehicleType { get; set; } = VehicleType.Microbus;
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
        // Navigation properties
        public ICollection<Trip> Trips { get; set; }
    }
}
