using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CourseAll.API.Dtos;
using CourseAll.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CourseAll.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [Route("login-user")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginDto loginUser)
        {
            var user = await _repo.LoginUser(loginUser);
            if(user == null)
                return Unauthorized();

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "user")
            };

            byte[] keybytes = Encoding.ASCII.GetBytes(_config.GetSection("Jwt:Key").Value);

            var key = new SymmetricSecurityKey(keybytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMonths(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new 
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        [Route("login-company")]
        [HttpPost]
        public async Task<IActionResult> LoginCompany(LoginDto loginDto)
        {
            var company = await _repo.LoginCompany(loginDto);
            if(company == null)
                return Unauthorized();

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, company.Id.ToString()),
                new Claim(ClaimTypes.Name, company.Name),
                new Claim(ClaimTypes.Role, "company")                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_config.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMonths(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new 
            {
                token = tokenHandler.WriteToken(token)
            });
        }        
    
        [Route("register-user")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUser)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(await _repo.UserExists(registerUser.Email) 
                || await _repo.UserExists(registerUser.Name))            
                return BadRequest();            

            var result = await _repo.RegisterUser(registerUser);

            if(!result)        
                return BadRequest();            
            return StatusCode(201);
        }
    
        [Route("register-company")]
        [HttpPost]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyDto registerCompany)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(await _repo.CompanyExists(registerCompany.Email)
                || await _repo.CompanyExists(registerCompany.Name))
                return BadRequest();

            var result = await _repo.RegisterCompany(registerCompany);

            if(!result)
                return BadRequest();
            return StatusCode(201);                                         
        }
    }
}