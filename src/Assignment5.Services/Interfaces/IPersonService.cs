using Assignment5.Domain.Entities;

namespace Assignment5.Application.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<Person> GetMaleMembers();
        Person GetOldestMember();
        IEnumerable<string> GetMemberFullNames();
        byte[] GetMembersAsExcelFile();
        IEnumerable<Person> FilterMembersByBirthYear(int year, string comparisonType);
        IEnumerable<Person> GetAll();
        void Create(Person person);
        void Update(Person person);
        void Delete(Guid id);
        Person GetPersonById(Guid id);

    }
}
