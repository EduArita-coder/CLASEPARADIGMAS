using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonsApp.Constants;
using PersonsApp.Dtos.Persons;
using PersonsApp.Entities;
using PersonsApp.Services.Persons;

namespace PersonsApp.Controllers
{
    [Route("api/person")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.NORMAL_USER}")]
        public async Task<ActionResult> GetPage(
            string searchTerm = "", int page = 1, int pageSize = 10 
        )
        {
            var response = await _personService.GetPageAsync(searchTerm, page, pageSize);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}, {RolesConstant.NORMAL_USER}")]

        public async Task<ActionResult> GetOne(string id)
        {
            var result = await _personService.GetOneByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]


        public async Task<ActionResult> Create(PersonCreateDto dto)
        {
            var result = await _personService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]
        

        public async Task<ActionResult> Update(string id, PersonEditDto dto)
        {
            var result = await _personService.EditAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RolesConstant.ADMIN}")]


        public async Task<ActionResult> Delete(string id)
        {
            var result = await _personService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}