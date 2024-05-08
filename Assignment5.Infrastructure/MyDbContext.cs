using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;

namespace Assignment5.Infrastructure
{
    public class MyDbContext
    {
        public static  List<Person> PersonList()
        {
            return new List<Person>
            {
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Mai Trong",
                LastName = "Hieu",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 04, 02),
                PhoneNumber = "0943317918",
                BirthPlace = "Ha Tinh",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Truong Trong",
                LastName = "Hoa",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 05, 12),
                PhoneNumber = "0941829321",
                BirthPlace = "Thanh Hoa",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Nguyen Minh",
                LastName = "Anh",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(2001,09,12),
                PhoneNumber = "0941234921",
                BirthPlace = "Ha Noi",
                IsGraduated = true
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Hoang Nhat",
                LastName = "Minh",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 06, 12),
                PhoneNumber = "0943334921",
                BirthPlace = "Ha Nam",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "La Thien",
                LastName = "Vu",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2000, 07, 13),
                PhoneNumber = "0943334123",
                BirthPlace = "Ha Noi",
                IsGraduated = true
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Nguyen Ngoc",
                LastName = "Quang",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1999, 08, 12),
                PhoneNumber = "0941134921",
                BirthPlace = "Nghe An",
                IsGraduated = true
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Le Viet",
                LastName = "Hoang",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(2003, 09, 22),
                PhoneNumber = "0943345921",
                BirthPlace = "Ha Giang",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Mai Duc",
                LastName = "Huy",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(2004, 09, 22),
                PhoneNumber = "0943317021",
                BirthPlace = "Ha Noi",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Trinh Duc",
                LastName = "Huy",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(2007, 09, 22),
                PhoneNumber = "0943317421",
                BirthPlace = "Ha Noi",
                IsGraduated = false
                },
                new Person
                {
                Id = Guid.NewGuid(),
                FirstName = "Trinh Duc",
                LastName = "Hoang",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(2007, 09, 22),
                PhoneNumber = "0943327021",
                BirthPlace = "Ha Noi",
                IsGraduated = false
                }
            };
        }
    }
}
