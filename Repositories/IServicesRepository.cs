using System.Collections.Generic;
using System.Threading.Tasks;
using CourseAll.API.Dtos;
using CourseAll.API.Models;

namespace CourseAll.API.Repositories
{
    public interface IServicesRepository
    {
        Task<List<Service>> GetServices(Header header);
        Task<Service> GetService(int id);
        Task<bool> AddService(Service service);
        Task<bool> DeleteService(int companyId, int serviceId);
        Task<Service> ModifyService(int companyId, int serviceId);
    }
}