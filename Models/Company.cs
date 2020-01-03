using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseAll.API.Models
{
    public class Company
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Name length must be more than '3'.")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Enter valid email adsress!")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Enter valid phone number")]
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Address { get; set; }
        public ICollection<Service> Services { get; set; }
        public string Tags { get; set; }
        public int Rating { get; set; }
        public byte[] Logo { get; set; }
    }
}