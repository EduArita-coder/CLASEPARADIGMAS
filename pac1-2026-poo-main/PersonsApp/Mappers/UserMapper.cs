using PersonsApp.Dtos.Users;
using PersonsApp.Entities;

namespace PersonsApp.Mappers
{
    public static class UserMapper
    {
        public static UserEntity CreateDtoToEntity(UserCreateDto dto)
        {
            return new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                UserName = dto.Email,
            };
        }

        public static UserEntity EditDtoToEntity(UserEntity entity, UserEditDto dto)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.BirthDate = dto.BirthDate;
            entity.Email = dto.Email;
            entity.UserName = dto.Email;
            return entity;
        }

        public static List<UserDto> ListEntityToListDto(List<UserEntity> entities)
        {
            return  entities.Select(entity => new UserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                BirthDate = entity.BirthDate,
                Email = entity.Email,
            }).ToList();
        }

        public static UserDto EntityToDto(UserEntity entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                BirthDate = entity.BirthDate,
                Email = entity.Email,
            };
        }
    }
}