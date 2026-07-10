using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Roles;

namespace PersonsApp.Services.Roles
{
    public interface IRoleService
    {
        Task<ResponseDto<PageDto<List<RoleDto>>>> GetPageAsync(
            string searchTerm = "", int page = 1, int pageSize = 10);

        Task<ResponseDto<RoleDto>> GetOneAsync(string id);
        Task<ResponseDto<RoleActionResponseDto>> CreateAsync(RoleCreateDto dto);
        Task<ResponseDto<RoleActionResponseDto>> EditAsync(string id, RoleEditDto dto);
        Task<ResponseDto<RoleActionResponseDto>> DeleteAsync(string id);
    }
}