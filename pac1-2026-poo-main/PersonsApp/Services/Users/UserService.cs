using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonsApp.Constants;
using PersonsApp.Database;
using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Users;
using PersonsApp.Entities;
using PersonsApp.Mappers;

namespace PersonsApp.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly PersonsDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public UserService(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            PersonsDbContext context,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        public async Task<ResponseDto<PageDto<List<UserDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            page = Math.Abs(page);
            pageSize = Math.Abs(pageSize);

            pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
            pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<UserEntity> usersQuery = _context.Users;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                usersQuery = usersQuery
                .Where(x => (x.FirstName + " " + x.LastName + " " + x.Email)
                    .Contains(searchTerm));
            }

            int totalRows = await usersQuery.CountAsync();

            var usersEntity = await usersQuery
                .OrderBy(x => x.FirstName)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            return new ResponseDto<PageDto<List<UserDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = new PageDto<List<UserDto>>
                {
                    CurrentPage = page == 0 ? 1 : page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = UserMapper.ListEntityToListDto(usersEntity),
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                        page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        public async Task<ResponseDto<UserDto>> GetOneAsync(string id)
        {
            var userEntity = await _context.Users
            .FirstOrDefaultAsync(r => r.Id == id);

            if (userEntity is null)
            {
                return new ResponseDto<UserDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false,
                };
            }

            return new ResponseDto<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Status = true,
                Data = UserMapper.EntityToDto(userEntity)
            };
        }

        public async Task<ResponseDto<UserActionResponseDto>> CreateAsync(UserCreateDto dto)
        {
            (bool flowControl, ResponseDto<UserActionResponseDto> value) = await ValidateRoles(dto);
            if (!flowControl)
            {
                return value;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = UserMapper.CreateDtoToEntity(dto);
                var createRsult = await _userManager.CreateAsync(user, dto.Password);

                if (!createRsult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", createRsult
                            .Errors.Select(e => e.Description))
                    };
                }

                // Asignar roles
                if (dto.Roles != null && dto.Roles.Any())
                {
                    var addRolesResult = await _userManager
                        .AddToRolesAsync(user, dto.Roles);

                    if (!addRolesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();

                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = string.Join(", ", addRolesResult
                                .Errors.Select(e => e.Description))
                        };
                    }
                }

                await transaction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.CREATED,
                    Status = true,
                    Message = HttpMessageResponse.REGISTER_CREATED,
                    Data = new UserActionResponseDto
                    {
                        Id = user.Id
                    }

                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = $"Error interno en el servidor: {e.Message}"
                };
            }
        }

        private async Task<(bool flowControl, ResponseDto<UserActionResponseDto> value)> ValidateRoles(UserCreateDto dto)
        {
            if (dto.Roles != null && dto.Roles.Any())
            {
                var existingRoles = await _roleManager
                    .Roles.Select(r => r.Name).ToListAsync();

                var invalidRoles = dto.Roles.Except(existingRoles);

                if (invalidRoles.Any())
                {
                    return (flowControl: false, value: new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Roles no permitidos: {string.Join(", ", invalidRoles)}"
                    });
                }
            }

            return (flowControl: true, value: null);
        }

        public async Task<ResponseDto<UserActionResponseDto>> EditAsync(string id, UserEditDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            (bool flowControl, ResponseDto<UserActionResponseDto> value) = await ValidateRoles(dto);
            if (!flowControl)
            {
                return value;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userEntity = UserMapper.EditDtoToEntity(user, dto);

                var updatedResult = await _userManager.UpdateAsync(userEntity);
                if (!updatedResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", updatedResult.Errors.Select(e => e.Description))
                    };
                }
                if (dto.Roles is not null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(userEntity);
                    var rolesToAdd = dto.Roles.Except(currentRoles).ToList();
                    var rolesToremover = currentRoles.Except(dto.Roles).ToList();

                    if (rolesToAdd.Any())
                    {
                        var addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                        if (addRolesResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return new ResponseDto<UserActionResponseDto>
                            {
                                StatusCode = HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = string.Join(", ", addRolesResult.Errors.Select(e => e.Description))

                            };
                        }
                    }

                    if (rolesToremover.Any())
                    {
                        var removeresult = await _userManager.RemoveFromRolesAsync(userEntity, rolesToremover);
                        if (removeresult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return new ResponseDto<UserActionResponseDto>
                            {
                                StatusCode = HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = string.Join(", ", removeresult.Errors.Select(e => e.Description))
                            };
                        }
                    }
                }
                await transaction.CommitAsync();
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = HttpMessageResponse.REGISTER_UPDATED,
                    Data = new UserActionResponseDto
                    {
                        Id = id
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = $"Error interno en el servidor : {ex.Message}"
                };
            }
            ;
        }

        public async Task<ResponseDto<UserActionResponseDto>> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                };
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var removerolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (removerolesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = string.Join(", ", removerolesResult.Errors.Select(e => e.Description))
                        };
                    }
                }
                var deleteUserResult = await _userManager.DeleteAsync(user);
                if(!deleteUserResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = string.Join(", ", deleteUserResult.Errors.Select(e => e.Description))
                        };
                }
                await transaction.CommitAsync();
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = HttpMessageResponse.REGISTER_UPDATED,
                    Data = new UserActionResponseDto
                    {
                        Id = id
                    }
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = $"Error interno en el servidor : {e.Message}"
                };
            }
            
        }
    }
}