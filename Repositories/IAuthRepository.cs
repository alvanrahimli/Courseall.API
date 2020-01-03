using System.Threading.Tasks;
using CourseAll.API.Dtos;
using CourseAll.API.Models;

namespace CourseAll.API.Repositories
{
    public interface IAuthRepository
    {
        Task<User> LoginUser(LoginDto loginUser);
        Task<Company> LoginCompany(LoginDto loginDto);
        Task<bool> RegisterUser(RegisterUserDto registerUser);
        Task<bool> RegisterCompany(RegisterCompanyDto registerCompany);
        Task<bool> UserExists(string parameter);
        Task<bool> CompanyExists(string parameter);
    }
}