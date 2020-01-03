using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseAll.API.Data;
using CourseAll.API.Dtos;
using CourseAll.API.Helpers;
using CourseAll.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAll.API.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly DataContext _context;
        public CompaniesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetCompanies(Header header)
        {
            var companies = from company in _context.Companies
                            select company;

            if(header.Filters != null)
            {
                foreach(var filter in header.Filters)
                {
                    switch(filter.Name)
                    {
                        case "name":
                            companies = companies.Where(c => c.Name
                                .Contains(filter.Parameters[0].ToString()));
                            break;
                        case "type":
                            companies = companies.Where(c => c.Tags
                                .Contains(filter.Parameters[0].ToString()));
                            break;
                        case "address":
                            companies = companies.Where(c => c.Address
                                .Contains(filter.Parameters[0].ToString()));
                            break;
                        default:
                            break;
                    }
                }
            }
            
            if(header.SortingType != null)
            {
                switch(header.SortingType)
                {
                    case "name_desc":
                        companies = companies.OrderByDescending(c => c.Name);
                        break;
                    case "name_asc":
                        companies = companies.OrderBy(c => c.Name);
                        break;
                    case "rating_desc":
                        companies = companies.OrderByDescending(c => c.Rating);
                        break;
                    case "rating_asc":
                        companies = companies.OrderBy(c => c.Rating);
                        break;
                    default:
                        break;
                }
            }

            var companiesReturning = await Helpers.PaginatedList<Company>
                .CreateAsync(companies, header.PageNum, header.PageSize);
                
            return companiesReturning;
        }

        public async Task<Company> GetCompany(int id)
        {
            var result = await _context.Companies
                .Include(c => c.Services).FirstOrDefaultAsync(c => c.Id == id);

            if(result == null)
                return null;

            result.PasswordHash = null;
            return result;
        }

        public async Task<Company> ModifyCompany(int id, Company company)
        {
            company.Id = id;
            _context.Companies.Update(company);

            var result = await _context.SaveChangesAsync();

            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> DeleteCompany(int id)        
        {
            Company company = new Company()
            {
                Id = id
            };

            _context.Companies.Remove(company);
            var result = await _context.SaveChangesAsync();

            return true ? result == 1 : false;
        }

        public async Task<bool> AddRating(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            company.Rating += 1;

            _context.Companies.Update(company);

            var result = await _context.SaveChangesAsync();

            return true ? result == 1 : false;
        }
    }
}