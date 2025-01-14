using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.RequestedTripVM;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Implementation;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
    public class RequestedTripService : IRequestedTripService
    {
        private readonly IRequestedTripRepository _requestedTripRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public RequestedTripService(IRequestedTripRepository requestedTripRepository, IMapper mapper, IStudentRepository studentRepository)
        {
            _requestedTripRepository = requestedTripRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public bool CreatRequestedTrip(RequestNowVM requestNowVM, string userId)
        {
            var studentId = _studentRepository.GetByUserIdAsync(userId).Result.StudentId;
            requestNowVM.StudentId = studentId;
            var requestedTrip = _mapper.Map<RequestedTrip>(requestNowVM);
            return _requestedTripRepository.CreateAsync(requestedTrip).Result;
        }
    }
}
