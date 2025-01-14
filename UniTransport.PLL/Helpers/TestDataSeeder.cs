using Microsoft.AspNetCore.Identity;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.PLL.Helpers
{
    public static class TestDataSeeder
    {
        public static async Task SeedTestDataAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Only seed if no vehicles exist
            if (!context.Vehicles.Any())
            {
                // Seed Vehicles
                var vehicles = new List<Vehicle>
                {
                    new Vehicle
                    {
                        LicensePlate = "ABC123",
                        VehicleType = VehicleType.Microbus,
                        Capacity = 50,
                        IsActive = true
                    },
                    new Vehicle
                    {
                        LicensePlate = "XYZ789",
                        VehicleType = VehicleType.Microbus,
                        Capacity = 45,
                        IsActive = true
                    },
                    new Vehicle
                    {
                        LicensePlate = "DEF456",
                        VehicleType = VehicleType.Microbus,
                        Capacity = 40,
                        IsActive = false
                    }
                };

                context.Vehicles.AddRange(vehicles);
                await context.SaveChangesAsync();

                // Seed Test Users (Students)
                var students = new[]
                {
                    new Student
                    {
                        User = new User
                        {
                            UserName = "student1",
                            Email = "student1@test.com",
                            FirstName = "John",
                            LastName = "Doe",
                            PhoneNumber = "1234567890",
                            EmailConfirmed = true
                        }
                    },
                    new Student
                    {
                        User = new User
                        {
                            UserName = "student2",
                            Email = "student2@test.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            PhoneNumber = "0987654321",
                            EmailConfirmed = true
                        }
                    }
                };

                foreach (var student in students)
                {
                    if (string.IsNullOrEmpty(student.User?.Email))
                    {
                        continue;
                    }

                    var existingUser = await userManager.FindByEmailAsync(student.User.Email);
                    if (existingUser == null)
                    {
                        var result = await userManager.CreateAsync(student.User, "Student@123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(student.User, "Student");
                            context.Students.Add(student);
                        }
                        else
                        {
                            // Handle error
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"Error creating user: {error.Description}");
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();

                // Seed Trips
                var trips = new List<Trip>
                {
                    new Trip
                    {
                        VehicleId = vehicles[0].VehicleId,
                        DepartureLocation = "University Main Gate",
                        ArrivalLocation = "Campus A",
                        DepartureTime = DateTime.Now.AddDays(1).Date.AddHours(8), // Tomorrow 8 AM
                        ArrivalTime = DateTime.Now.AddDays(1).Date.AddHours(9), // Tomorrow 9 AM
                        Price = 25.00,
                        AvailableSeats = vehicles[0].Capacity,
                        TripStatus = TripStatus.Active
                    },
                    new Trip
                    {
                        VehicleId = vehicles[1].VehicleId,
                        DepartureLocation = "University Main Gate",
                        ArrivalLocation = "Campus B",
                        DepartureTime = DateTime.Now.AddDays(1).Date.AddHours(10), // Tomorrow 10 AM
                        ArrivalTime = DateTime.Now.AddDays(1).Date.AddHours(11), // Tomorrow 11 AM
                        Price = 30.00,
                        AvailableSeats = vehicles[1].Capacity,
                        TripStatus = TripStatus.Active
                    },
                    new Trip
                    {
                        VehicleId = vehicles[0].VehicleId,
                        DepartureLocation = "University Main Gate",
                        ArrivalLocation = "Campus C",
                        DepartureTime = DateTime.Now.AddDays(2).Date.AddHours(9), // Day after tomorrow 9 AM
                        ArrivalTime = DateTime.Now.AddDays(2).Date.AddHours(10), // Day after tomorrow 10 AM
                        Price = 35.00,
                        AvailableSeats = vehicles[0].Capacity,
                        TripStatus = TripStatus.Active
                    }
                };

                context.Trips.AddRange(trips);
                await context.SaveChangesAsync();

                // Seed some bookings
                var bookings = new List<Booking>
                {
                    new Booking
                    {
                        TripId = trips[0].TripId,
                        StudentId = students[0].StudentId,
                        BookingTime = DateTime.Now,
                        IsCancelled = false
                    },
                    new Booking
                    {
                        TripId = trips[0].TripId,
                        StudentId = students[1].StudentId,
                        BookingTime = DateTime.Now,
                        IsCancelled = false
                    }
                };

                context.Bookings.AddRange(bookings);

                // Update available seats for the first trip
                trips[0].AvailableSeats -= 2; // Subtract total booked seats
                context.Trips.Update(trips[0]);

                await context.SaveChangesAsync();
            }
        }
    }
}
