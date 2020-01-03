using System.Collections.Generic;
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
    [Authorize]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesRepository _repo;
        public CompaniesController(ICompaniesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]        
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanies(Header header)
        {
            if(!ModelState.IsValid)
                return BadRequest("Please specify valid Header object!");

            var companies = await _repo.GetCompanies(header);

            if(companies == null)
                return NotFound();

            return Ok(companies);    
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany(int id)
        {
            if(id <= 0)
                return BadRequest("Company Id must be positive integer.");

            var result = await _repo.GetCompany(id);

            if(result == null)
                return NotFound();
            return Ok(result);
        }                

        [HttpPut]
        [Route("modify")]
        public async Task<IActionResult> ModifyCompany(Company company)
        {
            var id = Helper.GetId(Request.Headers["Authorization"]);

            if(company.Id != id)
                return BadRequest("STOP IT, GET SOME HELP! lol");

            var result = await _repo.ModifyCompany(id, company);

            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteCompany()
        {
            var id = Helper.GetId(Request.Headers["Authorization"]);

            var result = await _repo.DeleteCompany(id);

            if(result == true)
                return Ok("Company deleted succesfully.");
            return StatusCode(500);
        }

        [HttpPost]
        [Route("addrating")]
        public async Task<IActionResult> AddRating(int Id)
        {
            return Ok(await _repo.AddRating(Id)); 
        }
    }
}