using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CourseAll.API.Dtos
{
    public class RegisterCompanyDto
    {
        [Required]
        [StringLength(255, ErrorMessage = "Name must be between 3 and 255 characters", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email entry!")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number entry!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please specify valid password!")]
        [StringLength(255, ErrorMessage = "Password must be between 8 and 255 characters", MinimumLength = 8)]
        public string Password { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public string Tags { get; set; }
        public IFormFile Logo { get; set; }
    }
}