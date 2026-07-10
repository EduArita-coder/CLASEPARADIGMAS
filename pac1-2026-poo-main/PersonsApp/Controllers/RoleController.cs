using Microsoft.AspNetCore.Mvc;
using PersonsApp.Dtos.Roles;
using PersonsApp.Services.Roles;

namespace PersonsApp.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(
            IRoleService roleService
        )
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPage(
            string searchTerm = "", int page = 1, int pageSize = 10 
        )
        {
            var response = await _roleService.GetPageAsync(searchTerm, page, pageSize);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(string id)
        {
            var result = await _roleService.GetOneAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateDto dto)
        {
            var result = await _roleService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, RoleEditDto dto)
        {
            var result = await _roleService.EditAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _roleService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}