using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseAll.API.Helpers;

namespace CourseAll.API.Dtos
{
    public class Header
    {
        [Required]
        public int PageNum { get; set; }
        [Required]
        public int PageSize { get; set; }
        public List<Filter> Filters { get; set; }
        public string SortingType { get; set; }
    }
}