using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseAll.API.Data;
using CourseAll.API.Dtos;
using CourseAll.API.Models;
using Microsoft.EntityFrameworkCore;
using CourseAll.API.Helpers;
using System;

namespace CourseAll.API.Repositories
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly DataContext _context;
        public ServicesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetServices(Header header)
        {
            var services =  from service in _context.Services
                            select service;

            if(header.Filters != null)
            {
                foreach (var filter in header.Filters)
                {
                    switch(filter.Name)
                    {                        
                        case "price":
                            decimal minPrice, maxPrice;
                            try
                            {
                                minPrice = Convert.ToInt32(filter.Parameters[0]);
                                maxPrice = Convert.ToInt32(filter.Parameters[1]);
                                services = services.Where(s => s.Price > minPrice && s.Price < maxPrice);
                            }
                            catch {}                                                        
                            break;
                        case "type":
                            var type = filter.Parameters[0].ToString();
                            services = services.Where(s => s.Name.ToLower()
                                .Contains(type) || s.Description.ToLower().Contains(type) || s.Tags
                                .ToLower().Contains(type));
                            break;
                        case "rating":
                            var minRating = Convert.ToInt32(filter.Parameters[0]);
                            var maxRating = Convert.ToInt32(filter.Parameters[1]);
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
                    case "price_asc":
                        services = services.OrderBy(s => s.Price);
                        break;
                    case "price_desc":
                        services = services.OrderByDescending(s => s.Price);
                        break;
                    case "rating_asc":
                        services = services.OrderBy(s => s.Rating);
                        break;
                    case "rating_desc":
                        services = services.OrderByDescending(s => s.Rating);
                        break;
                }
            }

            var servicesToReturn = await Helpers.PaginatedList<Service>
                .CreateAsync(services, header.PageNum, header.PageSize);

            return servicesToReturn;
        }        

        async public Task<Service> GetService(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

            if(service == null)
                return null;
            
            return service;
        }

        public async Task<bool> AddService(Service service)
        {
            await _context.Services.AddAsync(service);
            var result = await _context.SaveChangesAsync();

            return true ? result == 1 : false;
        }

        public async Task<bool> DeleteService(int companyId, int serviceId)
        {
            var result = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId && s.CompanyId == companyId);
            
            _context.Services.Remove(result);

            var res = await _context.SaveChangesAsync();

            return true ? res == 1 : false;
        }

        public Task<Service> ModifyService(int companyId, int serviceId)
        {
            throw new NotImplementedException();
        }
    }
}