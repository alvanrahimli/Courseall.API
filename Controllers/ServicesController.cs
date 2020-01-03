using System.Threading.Tasks;
using CourseAll.API.Dtos;
using CourseAll.API.Helpers;
using CourseAll.API.Models;
using CourseAll.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseAll.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesRepository _repo;
        public ServicesController(IServicesRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetServices(Header header)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.GetServices(header);

            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            if(id < 0)
                return BadRequest("Not today mate <3");

            var result = await _repo.GetService(id);

            if(result == null)
                return NotFound();
            
            return Ok(result);
        }
    
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddService(Service service)
        {
            string role = Helper.GetRole(Request.Headers["Authorization"]);
            if(role != "company")
                return BadRequest("Only company accounts can 'delete' services");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.AddService(service);

            if(result)
                return StatusCode(201);
            return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            int companyId = Helper.GetId(Request.Headers["Authorization"]);
            string role = Helper.GetRole(Request.Headers["Authorization"]);

            if(role != "company")
                return BadRequest("Only company accounts can 'delete' services");

            var result = await _repo.DeleteService(serviceId, companyId);

            return result ? Ok() : StatusCode(500);
        }

        [HttpPut]
        [Route("modify")]
        public async Task<IActionResult> ModifyService(int serviceId)
        {
            int companyId = Helper.GetId(Request.Headers["Authorization"]);

            var service = await _repo.GetService(serviceId);

            if(service.CompanyId != companyId)
                return BadRequest();
            
            var result = await _repo.ModifyService(companyId, serviceId);

            if(result == null)
                return StatusCode(500);
            return Ok(result);
        }
    }
}