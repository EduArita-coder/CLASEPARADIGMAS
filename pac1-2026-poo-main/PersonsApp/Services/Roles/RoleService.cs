using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonsApp.Constants;
using PersonsApp.Database;
using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Roles;
using PersonsApp.Entities;
using PersonsApp.Mappers;

namespace PersonsApp.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly PersonsDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public RoleService(
            RoleManager<RoleEntity> roleManager,
            PersonsDbContext context,
            IConfiguration configuration
        )
        {
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            PAGE_SIZE = _configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = _configuration.GetValue<int>("PageSizeLimit");
        }


        public async Task<ResponseDto<PageDto<List<RoleDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            page = Math.Abs(page);
            pageSize = Math.Abs(pageSize);

            pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
            pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<RoleEntity> rolesQuery = _context.Roles;
            
            if(!string.IsNullOrEmpty(searchTerm))
            {
                rolesQuery = rolesQuery
                .Where( x => (x.Name + " " + x.Description)
                    .Contains(searchTerm) );
            }

            int totalRows = await rolesQuery.CountAsync();

            var rolesEntity = await rolesQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            return new ResponseDto<PageDto<List<RoleDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = new PageDto<List<RoleDto>>
                {
                    CurrentPage = page == 0 ? 1 : page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows/pageSize),
                    Items = RoleMapper.ListEntityToListDto(rolesEntity),
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT && 
                        page < (int)Math.Ceiling((double)totalRows/pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        public async Task<ResponseDto<RoleDto>> GetOneAsync(string id)
        {
            var roleEntity = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id);

            if(roleEntity is null)
            {
                return new ResponseDto<RoleDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false,                    
                };
            }

            return new ResponseDto<RoleDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Status = true,
                Data = new RoleDto
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    Description = roleEntity.Description,
                }
            };
        }

        public async Task<ResponseDto<RoleActionResponseDto>> CreateAsync(RoleCreateDto dto)
        {
           var roleEntity = RoleMapper.CreateDtoToEntity(dto);
           var result = await _roleManager.CreateAsync(roleEntity);

            if(!result.Succeeded)
            {
                return new ResponseDto<RoleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                };
            }

            return new ResponseDto<RoleActionResponseDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Data = RoleMapper.EntityToActionResponseDto(roleEntity)
            };
        }        

        public async Task<ResponseDto<RoleActionResponseDto>> EditAsync(string id, RoleEditDto dto)
        {
            var roleEntity = await _context.Roles.FirstOrDefaultAsync(p => p.Id == id);

            if(roleEntity is null)
            {
                return new ResponseDto<RoleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                };
            }

            var roleEntityUpdated = RoleMapper.EditDtoToEntity(roleEntity, dto);

            _context.Roles.Update(roleEntityUpdated);

            await _context.SaveChangesAsync();

            return new ResponseDto<RoleActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTER_UPDATED,
                Data = new RoleActionResponseDto
                {
                    Id = id
                }
            };
        }

        public async Task<ResponseDto<RoleActionResponseDto>> DeleteAsync(string id)
        {
            var roleEntity = await _context.Roles.FirstOrDefaultAsync(p => p.Id == id);

            if(roleEntity is null)
            {
                return new ResponseDto<RoleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                };
            }

            _context.Roles.Remove(roleEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<RoleActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTER_DELETED,
                Data = new RoleActionResponseDto
                {
                    Id = id
                }
            };
        }
    }
}