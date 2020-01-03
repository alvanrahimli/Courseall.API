using System.Collections.Generic;
using System.Threading.Tasks;
using CourseAll.API.Dtos;
using CourseAll.API.Models;

namespace CourseAll.API.Repositories
{
    public interface ICompaniesRepository
    {
        Task<List<Company>> GetCompanies(Header header);
        Task<Company> GetCompany(int id);
        Task<Company> ModifyCompany(int id, Company company);
        Task<bool> DeleteCompany(int id);
        Task<bool> AddRating(int id);
    }
}