using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
	public class TripService : ITripService
	{
		private readonly ITripRepository _tripRepository;
		private readonly IBookingRepository _bookingRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IMapper _mapper;
		public TripService(ITripRepository repository, IBookingRepository bookingRepository, IMapper mapper, IStudentRepository studentRepository)
		{
			_tripRepository = repository;
			_bookingRepository = bookingRepository;
			_mapper = mapper;
			_studentRepository = studentRepository;
		}

		//Get active incoming trips which its time after 10m from know
		public IEnumerable<BookTripsVM> GetIncomingTrips()
		{
			var incomingTrips = _tripRepository.GetIncomingTripsAsync().Result;
			return _mapper.Map<IEnumerable<BookTripsVM>>(incomingTrips);
		}
		public bool BookTrip(IEnumerable<BookTripsVM> trips, string userId)
		{
			foreach (var trip in trips)
			{
				if (trip.Isbooked)
				{
					try
					{
						var studentId = _studentRepository.GetByUserIdAsync(userId).Result.StudentId;
						var book = new Booking { TripId = trip.TripId, StudentId = studentId };
						var x = _bookingRepository.CreateAsync(book).Result;

						trip.AvailableSeats--;
						if (trip.AvailableSeats == 0)
						{
							var y = _tripRepository.UpdateTripStatusAsync(trip.TripId, TripStatus.Completed).Result;
						}
						var z = _tripRepository.UpdateAvailableSeatsAsync(trip.TripId, trip.AvailableSeats).Result;
						return true;
					}
					catch(Exception ex)
					{ 
						return false;
					}	
				}
			}
			return true;
		}
	}
}
