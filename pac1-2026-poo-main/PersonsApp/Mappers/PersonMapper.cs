using PersonsApp.Dtos.Countries;
using PersonsApp.Dtos.Persons;
using PersonsApp.Entities;

namespace PersonsApp.Mappers
{
    public static class PersonMapper
    {
        public static PersonEntity CreateDtoToEntity(PersonCreateDto dto)
        {
            return new PersonEntity
            {
                Id = Guid.NewGuid().ToString(),
                DNI = dto.DNI,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                CountryId = dto.CountryId
            };
        }

        public static PersonEntity EditDtoToEntity(PersonEntity entity, PersonEditDto dto)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.DNI = dto.DNI;
            entity.BirthDate = dto.BirthDate;
            entity.Gender = dto.Gender;
            entity.CountryId = dto.CountryId;
            return entity;
        }

        public static List<PersonDto> ListEntityToListDto(List<PersonEntity> entities)
        {
            return  entities.Select(person => new PersonDto
            {
                Id = person.Id,
                DNI = person.DNI,
                FirstName = person.FirstName,
                LastName = person.LastName,
                BirthDate = person.BirthDate,
                Gender = person.Gender,
                Country = new CountryOneDto
                {
                    Id = person.Country.Id,
                    Name = person.Country.Name
                }
            }).ToList();
        }
    }
}