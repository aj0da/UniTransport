using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.StudentVM;

namespace UniTransport.BLL.Service.Interface
{
    public interface IStudentService
    {
		StudentProfileVM GetStudentProfile(string userId);

	}
}
