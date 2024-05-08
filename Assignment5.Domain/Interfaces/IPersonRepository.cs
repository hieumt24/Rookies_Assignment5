using Assignment5.Domain.Entities;

namespace Assignment5.Domain.Interfaces
{
    public interface IPersonRepository 
    {
        IEnumerable<Person> GetAllPerson();
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(Guid id);
        Person GetPersonById(Guid id);
    }
}
