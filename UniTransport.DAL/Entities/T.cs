//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using UniTransport.DAL.Entities;
//using UniTransport.DAL.Enum;

//public class Booking
//{
//    public int BookingId { get; set; }
//    public DateTime BookingTime { get; set; } = DateTime.Now;
//    public bool IsCancelled { get; set; } = false;

//    // Navigation properties
//    public int StudentId { get; set; }
//    public Student Student { get; set; }
//    public int TripId { get; set; }
//    public Trip Trip { get; set; }
//}
//public class RequestedTrip
//{
//    public int RequestedTripId { get; set; }
//    public string DepartureLocation { get; set; }
//    public string ArrivalLocation { get; set; }
//    public DateTime DepartureTime { get; set; }
//    public DateTime RequestTime { get; set; } = DateTime.Now;
//    public bool IsPrivateRide { get; set; } = false;
//    public RequestedTripStatus Status { get; set; } = RequestedTripStatus.Pending;

//    // Navigation properties
//    public int StudentId { get; set; }
//    public Student Student { get; set; }
//}
//public enum RequestedTripStatus
//{
//    Pending,
//    Approved,
//    Rejected
//}
//public class Student
//{
//    public int StudentId { get; set; }
//    public int UniversityStudentId { get; set; }

//    // Navigation properties
//    public string UserId { get; set; }
//    public User User { get; set; }
//    public ICollection<Booking> Bookings { get; set; }
//    public ICollection<RequestedTrip> RequestedTrips { get; set; }
//}

//public class Trip
//{
//    public int TripId { get; set; }
//    public string DepartureLocation { get; set; }
//    public string ArrivalLocation { get; set; }
//    public DateTime DepartureTime { get; set; }
//    public DateTime ArrivalTime { get; set; }
//    public int AvailableSeats { get; set; }
//    public double Price { get; set; }
//    public DateTime CreatedAt { get; set; } = DateTime.Now;
//    public bool IsDeleted { get; set; } = false;
//    public TripStatus TripStatus { get; set; } = TripStatus.Active;

//    // Navigation properties
//    public int? RequestedTripId { get; set; }
//    public RequestedTrip RequestedTrip { get; set; }
//    public int VehicleId { get; set; }
//    public Vehicle Vehicle { get; set; }
//    public ICollection<Booking> Bookings { get; set; }
//}
//public enum TripStatus
//{
//    Active,
//    Cancelled,
//    Completed,
//}
//public class User : IdentityUser
//{
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public bool IsDeleted { get; set; } = false;
//    public string? Image { get; set; }
//    public DateTime CreatedAt { get; set; } = DateTime.Now;
//}
//public class Vehicle
//{
//    public int VehicleId { get; set; }
//    public string LicensePlate { get; set; }
//    public VehicleType VehicleType { get; set; } = VehicleType.Microbus;
//    public int Capacity { get; set; }
//    public bool IsActive { get; set; } = true;

//    // Navigation properties
//    public ICollection<Trip> Trips { get; set; }
//}
//public enum VehicleType
//{
//    Microbus,
//    Shuttle,       // Small shuttle bus
//    Bus,           // Regular bus
//    Minivan,       // Smaller vehicle
//    AccessibleVan  // Vehicle with accessibility features
//}
//public class ApplicationDbContext : IdentityDbContext<User>
//{
//    public DbSet<Student> Students { get; set; }
//    public DbSet<Trip> Trips { get; set; }
//    public DbSet<RequestedTrip> RequestedTrips { get; set; }
//    public DbSet<Vehicle> Vehicles { get; set; }
//    public DbSet<Booking> Bookings { get; set; }
//    public DbSet<User> Users { get; set; }

//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
//}



//using Microsoft.EntityFrameworkCore;
//using UniTransport.DAL.Database;
//using UniTransport.DAL.Entities;
//using UniTransport.DAL.Enum;

//public interface IGenericRepository<T> where T : class
//{
//    Task<IEnumerable<T>> GetAllAsync();
//    Task<T> GetByIdAsync(int id);
//    Task<bool> CreateAsync(T entity);
//    Task<bool> UpdateAsync(T entity);
//    Task<bool> DeleteAsync(int id);
//}

//public interface IBookingRepository : IGenericRepository<Booking>
//{
//    Task<Booking> GetByIdWithDetailsAsync(int id);
//    Task<IEnumerable<Booking>> GetBookingsByStudentIdAsync(int studentId);
//    Task<IEnumerable<Booking>> GetBookingsByTripIdAsync(int tripId);
//    Task<bool> CancelBookingAsync(int bookingId);
//}

//public interface IRequestedTripRepository : IGenericRepository<RequestedTrip>
//{
//    Task<RequestedTrip> GetByIdWithDetailsAsync(int id);
//    Task<IEnumerable<RequestedTrip>> GetRequestedTripsByStudentIdAsync(int studentId);
//    Task<IEnumerable<RequestedTrip>> GetPendingRequestsAsync();
//    Task<bool> UpdateStatusAsync(int requestedTripId, RequestedTripStatus status);
//}

//public interface IStudentRepository : IGenericRepository<Student>
//{
//    Task<Student> GetByIdWithDetailsAsync(int id);
//    Task<Student> GetByUniversityIdAsync(int universityStudentId);
//    Task<Student> GetByUserIdAsync(string userId);
//}

//public interface ITripRepository : IGenericRepository<Trip>
//{
//    Task<Trip> GetByIdWithDetailsAsync(int id);
//    Task<IEnumerable<Trip>> GetActiveTripsAsync();
//    Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId);
//    Task<bool> UpdateTripStatusAsync(int tripId, TripStatus status);
//    Task<bool> UpdateAvailableSeatsAsync(int tripId, int seats);
//}

//public interface IVehicleRepository : IGenericRepository<Vehicle>
//{
//    Task<Vehicle> GetByIdWithDetailsAsync(int id);
//    Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
//    Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType type);
//    Task<bool> ToggleVehicleStatusAsync(int vehicleId);
//}

//public class GenericRepository<T> : IGenericRepository<T> where T : class
//{
//    protected readonly ApplicationDbContext _context;
//    protected readonly DbSet<T> _dbSet;

//    public GenericRepository(ApplicationDbContext context)
//    {
//        _context = context;
//        _dbSet = context.Set<T>();
//    }

//    public virtual async Task<IEnumerable<T>> GetAllAsync()
//    {
//        return await _dbSet.ToListAsync();
//    }

//    public virtual async Task<T> GetByIdAsync(int id)
//    {
//        return await _dbSet.FindAsync(id);
//    }

//    public virtual async Task<bool> CreateAsync(T entity)
//    {
//        try
//        {
//            await _dbSet.AddAsync(entity);
//            return await _context.SaveChangesAsync() > 0;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public virtual async Task<bool> UpdateAsync(T entity)
//    {
//        try
//        {
//            _dbSet.Update(entity);
//            return await _context.SaveChangesAsync() > 0;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public virtual async Task<bool> DeleteAsync(int id)
//    {
//        var entity = await GetByIdAsync(id);
//        if (entity == null) return false;

//        try
//        {
//            _dbSet.Remove(entity);
//            return await _context.SaveChangesAsync() > 0;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}

//public class BookingRepository : GenericRepository<Booking>, IBookingRepository
//{
//    public BookingRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<Booking> GetByIdWithDetailsAsync(int id)
//    {
//        return await _context.Bookings
//            .Include(b => b.Student)
//                .ThenInclude(s => s.User)
//            .Include(b => b.Trip)
//                .ThenInclude(t => t.Vehicle)
//            .FirstOrDefaultAsync(b => b.BookingId == id);
//    }

//    public async Task<IEnumerable<Booking>> GetBookingsByStudentIdAsync(int studentId)
//    {
//        return await _context.Bookings
//            .Include(b => b.Trip)
//            .Where(b => b.StudentId == studentId)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Booking>> GetBookingsByTripIdAsync(int tripId)
//    {
//        return await _context.Bookings
//            .Include(b => b.Student)
//                .ThenInclude(s => s.User)
//            .Where(b => b.TripId == tripId)
//            .ToListAsync();
//    }

//    public async Task<bool> CancelBookingAsync(int bookingId)
//    {
//        var booking = await GetByIdAsync(bookingId);
//        if (booking == null) return false;

//        try
//        {
//            booking.IsCancelled = true;
//            return await UpdateAsync(booking);
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}

//public class RequestedTripRepository : GenericRepository<RequestedTrip>, IRequestedTripRepository
//{
//    public RequestedTripRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<RequestedTrip> GetByIdWithDetailsAsync(int id)
//    {
//        return await _context.RequestedTrips
//            .Include(rt => rt.Student)
//                .ThenInclude(s => s.User)
//            .FirstOrDefaultAsync(rt => rt.RequestedTripId == id);
//    }

//    public async Task<IEnumerable<RequestedTrip>> GetRequestedTripsByStudentIdAsync(int studentId)
//    {
//        return await _context.RequestedTrips
//            .Where(rt => rt.StudentId == studentId)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<RequestedTrip>> GetPendingRequestsAsync()
//    {
//        return await _context.RequestedTrips
//            .Include(rt => rt.Student)
//                .ThenInclude(s => s.User)
//            .Where(rt => rt.Status == RequestedTripStatus.Pending)
//            .ToListAsync();
//    }

//    public async Task<bool> UpdateStatusAsync(int requestedTripId, RequestedTripStatus status)
//    {
//        var requestedTrip = await GetByIdAsync(requestedTripId);
//        if (requestedTrip == null) return false;

//        try
//        {
//            requestedTrip.Status = status;
//            return await UpdateAsync(requestedTrip);
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}

//public class StudentRepository : GenericRepository<Student>, IStudentRepository
//{
//    public StudentRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<Student> GetByIdWithDetailsAsync(int id)
//    {
//        return await _context.Students
//            .Include(s => s.User)
//            .Include(s => s.Bookings)
//            .Include(s => s.RequestedTrips)
//            .FirstOrDefaultAsync(s => s.StudentId == id);
//    }

//    public async Task<Student> GetByUniversityIdAsync(int universityStudentId)
//    {
//        return await _context.Students
//            .Include(s => s.User)
//            .FirstOrDefaultAsync(s => s.UniversityStudentId == universityStudentId);
//    }

//    public async Task<Student> GetByUserIdAsync(string userId)
//    {
//        return await _context.Students
//            .Include(s => s.User)
//            .FirstOrDefaultAsync(s => s.UserId == userId);
//    }
//}

//public class TripRepository : GenericRepository<Trip>, ITripRepository
//{
//    public TripRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<Trip> GetByIdWithDetailsAsync(int id)
//    {
//        return await _context.Trips
//            .Include(t => t.Vehicle)
//            .Include(t => t.RequestedTrip)
//            .Include(t => t.Bookings)
//                .ThenInclude(b => b.Student)
//            .FirstOrDefaultAsync(t => t.TripId == id);
//    }

//    public async Task<IEnumerable<Trip>> GetActiveTripsAsync()
//    {
//        return await _context.Trips
//            .Include(t => t.Vehicle)
//            .Where(t => t.TripStatus == TripStatus.Active && !t.IsDeleted)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId)
//    {
//        return await _context.Trips
//            .Where(t => t.VehicleId == vehicleId && !t.IsDeleted)
//            .ToListAsync();
//    }

//    public async Task<bool> UpdateTripStatusAsync(int tripId, TripStatus status)
//    {
//        var trip = await GetByIdAsync(tripId);
//        if (trip == null) return false;

//        try
//        {
//            trip.TripStatus = status;
//            return await UpdateAsync(trip);
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public async Task<bool> UpdateAvailableSeatsAsync(int tripId, int seats)
//    {
//        var trip = await GetByIdAsync(tripId);
//        if (trip == null) return false;

//        try
//        {
//            trip.AvailableSeats = seats;
//            return await UpdateAsync(trip);
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}

//public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
//{
//    public VehicleRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<Vehicle> GetByIdWithDetailsAsync(int id)
//    {
//        return await _context.Vehicles
//            .Include(v => v.Trips)
//            .FirstOrDefaultAsync(v => v.VehicleId == id);
//    }

//    public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
//    {
//        return await _context.Vehicles
//            .Where(v => v.IsActive)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType type)
//    {
//        return await _context.Vehicles
//            .Where(v => v.VehicleType == type && v.IsActive)
//            .ToListAsync();
//    }

//    public async Task<bool> ToggleVehicleStatusAsync(int vehicleId)
//    {
//        var vehicle = await GetByIdAsync(vehicleId);
//        if (vehicle == null) return false;

//        try
//        {
//            vehicle.IsActive = !vehicle.IsActive;
//            return await UpdateAsync(vehicle);
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}

//public class UserRepository : GenericRepository<User>, IUserRepository
//{
//    public UserRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<User> GetByEmailAsync(string email)
//    {
//        try
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
//        }
//        catch
//        {
//            return null;
//        }
//    }

//    public async Task<bool> DeactivateUserAsync(string userId)
//    {
//        try
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null) return false;
//            user.IsDeleted = true;
//            return await UpdateAsync(user);
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}