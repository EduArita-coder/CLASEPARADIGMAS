using PersonsApp.Dtos.Countries;

namespace PersonsApp.Dtos.Persons
{
    public class PersonDto
    {
        public string Id { get; set; }
        public string DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public CountryOneDto Country { get; set; }

    }
}