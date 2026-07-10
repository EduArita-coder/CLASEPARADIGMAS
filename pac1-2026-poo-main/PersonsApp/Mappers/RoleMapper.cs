using PersonsApp.Dtos.Roles;
using PersonsApp.Entities;

namespace PersonsApp.Mappers
{
    public static class RoleMapper
    {
        public static RoleEntity CreateDtoToEntity(RoleCreateDto dto)
        {
            return new RoleEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static RoleActionResponseDto EntityToActionResponseDto(RoleEntity entity)
        {
            return new RoleActionResponseDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static RoleEntity EditDtoToEntity(RoleEntity entity, RoleEditDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            return entity;
        }

        public static List<RoleDto> ListEntityToListDto(List<RoleEntity> entities)
        {
            return  entities.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            }).ToList();
        }
    }
}