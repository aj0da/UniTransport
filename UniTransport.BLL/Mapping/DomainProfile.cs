using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.BookingVM;
using UniTransport.BLL.ModelVM.RequestedTripVM;
using UniTransport.BLL.ModelVM.StudentVM;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Trip, BookTripsVM>()
                .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Vehicle.LicensePlate));

            CreateMap<Booking, BookingStudentProfileVM>()
                .ForMember(dest => dest.DepartureLocation, opt => opt.MapFrom(src => src.Trip.DepartureLocation))
                .ForMember(dest => dest.ArrivalLocation, opt => opt.MapFrom(src => src.Trip.ArrivalLocation))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Trip.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.Trip.ArrivalTime))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Trip.Price))
                .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.Trip.TripStatus))
                .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Trip.Vehicle.LicensePlate));

            CreateMap<RequestedTrip, RequestedTripStudentProfileVM>();

            CreateMap<Student, StudentProfileVM>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.User.Image))
                .ForMember(dest => dest.StudentUserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings))
                .ForMember(dest => dest.RequestedTrips, opt => opt.MapFrom(src => src.RequestedTrips));

            CreateMap<RequestNowVM, RequestedTrip>()
            .ForMember(dest => dest.DepartureTime,
                       opt => opt.MapFrom(src => src.DepartureDate.Date + src.DepartureTime.TimeOfDay))
            .ForMember(dest => dest.RequestTime,
                       opt => opt.Ignore()) // Ignore RequestTime as it has a default value
            .ForMember(dest => dest.RequestedTripId,
                       opt => opt.Ignore()) // Ignore ID as it's likely auto-generated
            .ForMember(dest => dest.Status,
                       opt => opt.Ignore()) // Ignore Status to keep the default value
            .ForMember(dest => dest.Student,
                       opt => opt.Ignore()); // Ignore navigation properties for now

            //CreateMap<Trip, TripViewModel>()
            //    .ForMember(dest => dest.TripTime,
            //          opt => opt.MapFrom(src => $"{src.DepartureTime:HH:mm} - {src.ArrivalTime:HH:mm}"));

            CreateMap<Trip, TripViewModel>();
            CreateMap<TripViewModel, Trip>();
        }
    }
}
