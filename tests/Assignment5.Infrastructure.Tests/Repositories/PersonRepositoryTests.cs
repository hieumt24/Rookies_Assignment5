using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;
using Assignment5.Domain.Interfaces;
using Assignment5.Infrastructure.Repositories;

namespace Assignment5.Infrastructure.Tests.Repositories
{
    public class PersonRepositoryTests
    {
        private readonly PersonRepository _repository;

        public PersonRepositoryTests()
        {
            _repository = new PersonRepository();
        }

        [Fact]
        public void GetAllPerson_ShouldReturnAllPeople()
        {
            // Act
            var result = _repository.GetAllPerson();

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(PersonRepository.listPeople.Count, resultList.Count);

            for (int i = 0; i < resultList.Count; i++)
            {
                Assert.Equal(PersonRepository.listPeople[i].Id, resultList[i].Id);
                Assert.Equal(PersonRepository.listPeople[i].FirstName, resultList[i].FirstName);
                Assert.Equal(PersonRepository.listPeople[i].LastName, resultList[i].LastName);
            }
        }

        // Example method test: you would need to add similar tests for other methods in your PersonRepository
        // This is just a sample test. Your PersonRepository should have a corresponding method that this test is intended for.
        [Fact]
        public void AddPerson_ShouldCorrectlyAddPerson()
        {
            // Arrange
            var newPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 01, 01),
                PhoneNumber = "1234567890",
                BirthPlace = "New City",
                IsGraduated = false
            };

            // Act
            _repository.AddPerson(newPerson);

            // Assert
            var addedPerson = _repository.GetPersonById(newPerson.Id);
            Assert.NotNull(addedPerson);
            Assert.Equal(newPerson.FirstName, addedPerson.FirstName);
        }

        [Fact]
        public void AddPerson_ShouldThrowArgumentNullException_WhenPersonIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _repository.AddPerson(null));
            Assert.Equal("person", exception.ParamName);
        }

        [Fact]
        public void AddPerson_ShouldAddPerson_WhenPersonIsValid()
        {
            // Arrange
            var person = new Person
            {
                FirstName = "New",
                LastName = "Person",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Someplace",
                IsGraduated = false
            };
            var initialCount = PersonRepository.listPeople.Count;

            // Act
            _repository.AddPerson(person);

            // Assert
            Assert.Equal(initialCount + 1, PersonRepository.listPeople.Count);
            var addedPerson = PersonRepository.listPeople.Find(p => p.PhoneNumber == person.PhoneNumber);
            Assert.NotNull(addedPerson);
            Assert.NotEqual(Guid.Empty, addedPerson.Id);
            Assert.Equal(person.FirstName, addedPerson.FirstName);
        }

        [Fact]
        public void DeletePerson_ShouldRemovePerson_WhenPersonExists()
        {
            // Arrange
            var personToDelete = PersonRepository.listPeople.First(); // Assuming there is at least one person in the list
            var initialCount = PersonRepository.listPeople.Count;

            // Act
            _repository.DeletePerson(personToDelete.Id);

            // Assert
            Assert.Equal(initialCount - 1, PersonRepository.listPeople.Count);
            var personExists = PersonRepository.listPeople.Any(p => p.Id == personToDelete.Id);
            Assert.False(personExists);
        }

        [Fact]
        public void DeletePerson_NonExistingId_ThrowsArgumentException()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.DeletePerson(nonExistingId));
            Assert.Equal("Person not found. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void UpdatePerson_ShouldUpdatePersonDetails_WhenPersonExists()
        {
            // Arrange
            var personToUpdate = PersonRepository.listPeople.First(); // Assuming there's at least one person
            var updatedPerson = new Person
            {
                Id = personToUpdate.Id, // Existing ID
                FirstName = "Updated",
                LastName = "Person",
                Gender = GenderType.Female,
                DateOfBirth = new DateTime(1991, 1, 1),
                PhoneNumber = "0987654321",
                BirthPlace = "UpdatedPlace",
                IsGraduated = true
            };

            // Act
            _repository.UpdatePerson(updatedPerson);

            // Assert
            var updatedPersonFromRepo = PersonRepository.listPeople.FirstOrDefault(p => p.Id == updatedPerson.Id);
            Assert.NotNull(updatedPersonFromRepo);
            Assert.Equal(updatedPerson.FirstName, updatedPersonFromRepo.FirstName);
            Assert.Equal(updatedPerson.LastName, updatedPersonFromRepo.LastName);
            Assert.Equal(updatedPerson.Gender, updatedPersonFromRepo.Gender);
            Assert.Equal(updatedPerson.DateOfBirth, updatedPersonFromRepo.DateOfBirth);
            Assert.Equal(updatedPerson.PhoneNumber, updatedPersonFromRepo.PhoneNumber);
            Assert.Equal(updatedPerson.BirthPlace, updatedPersonFromRepo.BirthPlace);
            Assert.Equal(updatedPerson.IsGraduated, updatedPersonFromRepo.IsGraduated);
        }

        [Fact]
        public void UpdatePerson_NonExistingPerson_ThrowsArgumentException()
        {
            // Arrange
            var nonExistingPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "NonExisting",
                LastName = "Person",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "Unknown",
                IsGraduated = false
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _repository.UpdatePerson(nonExistingPerson));
            Assert.Equal("Person not found. (Parameter 'Id')", exception.Message);
        }

        [Fact]
        public void PersonExists_ShouldReturnTrue_WhenPersonExists()
        {
            // Arrange
            var existingId = PersonRepository.listPeople.First().Id; // Assuming there is at least one person in the list

            // Act
            bool exists = _repository.PersonExists(existingId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void PersonExists_ShouldReturnFalse_WhenPersonDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid(); // Id that does not exist in the listPeople

            // Act
            bool doesNotExist = _repository.PersonExists(nonExistingId);

            // Assert
            Assert.False(doesNotExist);
        }

        [Fact]
        public void GetPersonById_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            var expectedPerson = _repository.GetAllPerson().First(); // Assuming there's at least one person
            var existingId = expectedPerson.Id;

            // Act
            var person = _repository.GetPersonById(existingId);

            // Assert
            Assert.NotNull(person);
            Assert.Equal(expectedPerson.Id, person.Id);
            Assert.Equal(expectedPerson.FirstName, person.FirstName);
            Assert.Equal(expectedPerson.LastName, person.LastName);
            Assert.Equal(expectedPerson.Gender, person.Gender);
            Assert.Equal(expectedPerson.Age, person.Age);
        }

        [Fact]
        public void GetPersonById_NonExistingPerson_ThrowsArgumentException()
        {
            // Arrange
            var repository = new PersonRepository();
            var nonExistingId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.GetPersonById(nonExistingId));
            Assert.Equal("Person not found. (Parameter 'id')", exception.Message);
        }
    }
}