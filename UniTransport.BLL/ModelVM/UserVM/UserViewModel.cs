using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniTransport.BLL.ModelVM.UserVM
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Image { get; set; }
        
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
