using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;
using Assignment5.Domain.Interfaces;
using Assignment5.Infrastructure.Repositories;

namespace Assignment5.Infrastructure.Tests.Repositories
{
    public class PersonRepositoryTests
    {
        [Fact]
        public void AddPerson_NullPerson_ThrowsArgumentNullException()
        {
            // Arrange
            var repository = new PersonRepository();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => repository.AddPerson(null));
        }

        [Fact]
        public void AddPerson_ValidPerson_SuccessfullyAdded()
        {
            // Arrange
            var repository = new PersonRepository();
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };

            // Act
            repository.AddPerson(person);

            // Assert
            Assert.Contains(person, repository.GetAllPerson());
        }

        [Fact]
        public void DeletePerson_ExistingId_PersonRemoved()
        {
            // Arrange
            var repository = new PersonRepository();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };
            repository.AddPerson(person);

            // Act
            repository.DeletePerson(person.Id);

            // Assert
            Assert.DoesNotContain(person, repository.GetAllPerson());
        }

        [Fact]
        public void DeletePerson_NonExistingId_ThrowsArgumentException()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.DeletePerson(nonExistingId));
        }

        [Fact]
        public void GetAllPerson_Should_Return_All_People()
        {
            // Arrange
            var repository = new PersonRepository();

            // Act
            var people = repository.GetAllPerson();

            // Assert
            Assert.Equal(PersonRepository.listPeople, people);
        }

        [Fact]
        public void UpdatePerson_ExistingPerson_PersonUpdated()
        {
            // Arrange
            var repository = new PersonRepository();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };
            repository.AddPerson(person);
            var updatedPerson = new Person
            {
                Id = person.Id,
                FirstName = "Jane",
                LastName = "Doe",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(1995, 2, 2),
                PhoneNumber = "0987654321",
                BirthPlace = "New City",
                IsGraduated = false
            };

            // Act
            repository.UpdatePerson(updatedPerson);

            // Assert
            var retrievedPerson = repository.GetAllPerson().FirstOrDefault(p => p.Id == updatedPerson.Id);
            Assert.NotNull(retrievedPerson);
            Assert.Equal(updatedPerson.FirstName, retrievedPerson.FirstName);
            Assert.Equal(updatedPerson.LastName, retrievedPerson.LastName);
            Assert.Equal(updatedPerson.Gender, retrievedPerson.Gender);
            Assert.Equal(updatedPerson.DateOfBirth, retrievedPerson.DateOfBirth);
            Assert.Equal(updatedPerson.PhoneNumber, retrievedPerson.PhoneNumber);
            Assert.Equal(updatedPerson.BirthPlace, retrievedPerson.BirthPlace);
            Assert.Equal(updatedPerson.IsGraduated, retrievedPerson.IsGraduated);
        }

        [Fact]
        public void UpdatePerson_NonExistingPerson_ThrowsArgumentException()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.UpdatePerson(nonExistingPerson));
        }

        [Fact]
        public void PersonExists_ExistingId_ReturnsTrue()
        {
            // Arrange
            var repository = new PersonRepository();
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };
            repository.AddPerson(person);

            // Act
            var exists = repository.PersonExists(person.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void PersonExists_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingId = Guid.NewGuid();

            // Act
            var exists = repository.PersonExists(nonExistingId);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void GetPersonById_ExistingId_ReturnsPerson()
        {
            // Arrange
            var repository = new PersonRepository();
            var expectedPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Some City",
                IsGraduated = true
            };
            repository.AddPerson(expectedPerson);

            // Act
            var retrievedPerson = repository.GetPersonById(expectedPerson.Id);

            // Assert
            Assert.Equal(expectedPerson, retrievedPerson);
        }

        [Fact]
        public void GetPersonById_NonExistingId_ThrowsArgumentException()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.GetPersonById(nonExistingId));
        }
    }
}