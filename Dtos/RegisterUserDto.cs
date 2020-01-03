using System.ComponentModel.DataAnnotations;

namespace CourseAll.API.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Please specify valid 'Name' parameter")]
        [StringLength(255, ErrorMessage = "Name must be between 3 and 255 characters", MinimumLength = 3)]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email entry!")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone entry!")]
        public string Phone { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Password must be between 8 and 255 characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}