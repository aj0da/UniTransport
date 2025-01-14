using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.BLL.ModelVM.StudentVM
{
    public class StudentLoginVM
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me!!")]
        public bool RememberMe { get; set; }
    }
}
