using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CourseAll.API.Data;
using CourseAll.API.Dtos;
using CourseAll.API.Helpers;
using CourseAll.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CourseAll.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> LoginUser(LoginDto loginUser)
        {
            User user = null;

            if(loginUser.Parameter.Contains('@'))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Parameter.ToLower());
            }
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Name == loginUser.Parameter.ToLower());
            }

            if(user == null)
                return null;
            
            if(!Helper.IsValidPassword(loginUser.Password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public async Task<Company> LoginCompany(LoginDto loginCompany)
        {
            Company company = null;

            if(loginCompany.Parameter.Contains('@'))
            {
                company = await _context.Companies
                    .FirstOrDefaultAsync(u => u.Email == loginCompany.Parameter.ToLower());
            }
            else
            {
                company = await _context.Companies
                    .FirstOrDefaultAsync(u => u.Name == loginCompany.Parameter.ToLower());
            }

            if(company == null)
                return null;
            
            if(!Helper.IsValidPassword(loginCompany.Password, company.PasswordHash))
            {
                return null;
            }

            return company;
        }
    
        public async Task<bool> RegisterUser(RegisterUserDto registerUser)
        {
            registerUser.Name = registerUser.Name.ToLower();
            registerUser.Email = registerUser.Email.ToLower();
            
            User user = new User()
            {
                Name = registerUser.Name,
                Email = registerUser.Email,
                Phone = registerUser.Phone,
                PasswordHash = Helper.ComputeHash(registerUser.Password)
            };

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            if(result == 1)
                return true;
            return false;
        }

        public async Task<bool> RegisterCompany(RegisterCompanyDto registerCompany)
        {
            registerCompany.Name = registerCompany.Name.ToLower();
            registerCompany.Email = registerCompany.Email.ToLower();

            Company company = new Company()
            {
                Name = registerCompany.Name,
                Email = registerCompany.Email,
                Phone = registerCompany.Phone,
                Address = registerCompany.Address,
                PasswordHash = Helper.ComputeHash(registerCompany.Password),
                Tags = registerCompany.Tags,
                Rating = registerCompany.Rating
            };

            using (var memoryStream = new MemoryStream())
            {
                await registerCompany.Logo.CopyToAsync(memoryStream);
                company.Logo = memoryStream.ToArray();
            }

            await _context.Companies.AddAsync(company);
            var result = await _context.SaveChangesAsync();

            if(result == 1)
                return true;
            return false;
        }

        public async Task<bool> UserExists(string parameter)
        {
            User user = null;
            if(parameter.Contains('@'))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == parameter.ToLower());
            }
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Name == parameter.ToLower());
            }

            if(user == null)            
                return false;            
            return true;
        }

        public async Task<bool> CompanyExists(string parameter)
        {
            Company company = null;

            if(parameter.Contains('@'))
            {
                company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == parameter.ToLower());
            }
            else
            {
                company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == parameter.ToLower());
            }

            if(company == null)
                return false;
            return true;
        }
    }
}