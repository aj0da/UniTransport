using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.StudentVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
		private readonly IMapper _mapper;
		public StudentService(IStudentRepository studentRepository, IMapper mapper)
		{
			_studentRepository = studentRepository;
			_mapper = mapper;
		}

		public StudentProfileVM GetStudentProfile(string userId)
        {
            var student = _studentRepository.GetByUserIdAsync(userId).Result;
			return _mapper.Map<StudentProfileVM>(student);
		}
    }
}
