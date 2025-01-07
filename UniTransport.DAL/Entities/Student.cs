using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.DAL.Entities
{
    public class Student
    {
        [Key]   
        public int Id { get; set; }
        public int StudentId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<Booking> Bookings { get; set; }  
    }
}
