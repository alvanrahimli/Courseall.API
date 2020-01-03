using System.ComponentModel.DataAnnotations;

namespace CourseAll.API.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string Tags { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}