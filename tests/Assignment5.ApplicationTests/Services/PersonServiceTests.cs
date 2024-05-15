using Xunit;
using Moq;
using Assignment5.Application.Services;
using Assignment5.Domain.Interfaces;
using Assignment5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Assignment5.Domain.Enums;
using ClosedXML.Excel;

namespace Assignment5.Application.Tests.Services
{
    public class PersonServiceTests
    {
        private Mock<IPersonRepository> _mockPersonRepository;
        private PersonService _personService;

        public PersonServiceTests()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _personService = new PersonService(_mockPersonRepository.Object);
        }

        [Fact]
        public void Create_Calls_AddPerson_Method_Of_PersonRepository_Once()
        {
            // Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personService = new PersonService(mockPersonRepository.Object);
            var person = new Person();

            // Act
            personService.Create(person);

            // Assert
            mockPersonRepository.Verify(repo => repo.AddPerson(person), Times.Once);
        }

        [Fact]
        public void Delete_Calls_DeletePerson_Method_Of_PersonRepository_Once()
        {
            // Arrange
            var mockPersonRepository = new Mock<IPersonRepository>();
            var personService = new PersonService(mockPersonRepository.Object);
            var personId = Guid.NewGuid();

            // Act
            personService.Delete(personId);

            // Assert
            mockPersonRepository.Verify(repo => repo.DeletePerson(personId), Times.Once);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsMembersWithEqualYear()
        {
            // Arrange
            int year = 1990;
            string comparisonType = "equal";
            var mockPersons = new List<Person>
    {
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1990, 5, 15) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1991, 10, 20) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1989, 3, 5) }
    };
            var expectedFilteredPersons = mockPersons.Where(x => x.DateOfBirth.Year == year);
            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(mockPersons);
            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var filteredPersons = personService.FilterMembersByBirthYear(year, comparisonType);

            // Assert
            Assert.Equal(expectedFilteredPersons, filteredPersons);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsMembersWithGreaterYear()
        {
            // Arrange
            int year = 1990;
            string comparisonType = "greater";
            var mockPersons = new List<Person>
    {
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1991, 5, 15) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1989, 10, 20) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1995, 3, 5) }
    };
            var expectedFilteredPersons = mockPersons.Where(x => x.DateOfBirth.Year > year);
            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(mockPersons);
            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var filteredPersons = personService.FilterMembersByBirthYear(year, comparisonType);

            // Assert
            Assert.Equal(expectedFilteredPersons, filteredPersons);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsMembersWithLessYear()
        {
            // Arrange
            int year = 1990;
            string comparisonType = "less";
            var mockPersons = new List<Person>
    {
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1989, 5, 15) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1991, 10, 20) },
        new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1988, 3, 5) }
    };
            var expectedFilteredPersons = mockPersons.Where(x => x.DateOfBirth.Year < year);
            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(mockPersons);
            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var filteredPersons = personService.FilterMembersByBirthYear(year, comparisonType);

            // Assert
            Assert.Equal(expectedFilteredPersons, filteredPersons);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsAllMembersForDefault()
        {
            // Arrange
            int year = 1990;
            string comparisonType = "invalid";
            var mockPersons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1989, 5, 15) },
                new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1991, 10, 20) },
                new Person { Id = Guid.NewGuid(), DateOfBirth = new DateTime(1988, 3, 5) }
            };
            var expectedFilteredPersons = mockPersons;
            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(mockPersons);
            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var filteredPersons = personService.FilterMembersByBirthYear(year, comparisonType);

            // Assert
            Assert.Equal(expectedFilteredPersons, filteredPersons);
        }

        [Fact]
        public void GetAll_Returns_All_Persons_From_Repository()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson" },
                new Person { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Smith" },
                new Person { Id = Guid.NewGuid(), FirstName = "Charlie", LastName = "Brown" }
            };

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(persons.AsQueryable());

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetAll();

            // Assert
            Assert.Equal(persons.Count, result.Count());
            foreach (var person in persons)
            {
                Assert.Contains(person, result);
            }
        }

        [Fact]
        public void GetMaleMembers_Returns_Male_Persons_From_Repository()
        {
            // Arrange
            var malePersons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = GenderType.Male },
                new Person { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Smith", Gender = GenderType.Male }
            };

            var femalePersons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson", Gender = GenderType.Female },
                new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Gender = GenderType.Female }
            };

            var allPersons = malePersons.Concat(femalePersons);

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(allPersons.AsQueryable());

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetMaleMembers();

            // Assert
            Assert.Equal(malePersons.Count, result.Count());
            foreach (var person in malePersons)
            {
                Assert.Contains(person, result);
            }
            foreach (var person in femalePersons)
            {
                Assert.DoesNotContain(person, result);
            }
        }

        [Fact]
        public void GetMemberFullNames_Returns_Combined_FirstName_LastName()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson" }
            };

            var expectedFullNames = persons.Select(p => p.FirstName + " " + p.LastName);

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(persons.AsQueryable());

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetMemberFullNames();

            // Assert
            Assert.Equal(expectedFullNames, result);
        }

        [Fact]
        public void GetMembersAsExcelFile_Returns_Excel_File_With_Correct_Values()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = GenderType.Male, DateOfBirth = new DateTime(1990, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
                new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson", Gender = GenderType.Female, DateOfBirth = new DateTime(1995, 8, 20), PhoneNumber = "9876543210", BirthPlace = "Los Angeles", IsGraduated = false },
            };

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(persons);

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetMembersAsExcelFile();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);

            // Validate Excel contents
            using (var stream = new MemoryStream(result))
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var lastRow = worksheet.LastRowUsed().RowNumber();
                    Assert.Equal(persons.Count + 1, lastRow); // +1 for header row

                    // Validate specific cell values
                    for (int i = 0; i < persons.Count; i++)
                    {
                        Assert.Equal(persons[i].FirstName, worksheet.Cell(i + 2, 1).Value);
                        Assert.Equal(persons[i].LastName, worksheet.Cell(i + 2, 2).Value);
                        Assert.Equal(persons[i].Gender.ToString(), worksheet.Cell(i + 2, 3).Value);
                        Assert.Equal(persons[i].DateOfBirth.ToString("yyyy-MM-dd"), worksheet.Cell(i + 2, 4).Value);
                        Assert.Equal(persons[i].PhoneNumber, worksheet.Cell(i + 2, 5).Value);
                        Assert.Equal(persons[i].BirthPlace, worksheet.Cell(i + 2, 6).Value);
                        Assert.Equal(persons[i].IsGraduated ? "Yes" : "No", worksheet.Cell(i + 2, 7).Value);
                    }
                }
            }
        }

        [Fact]
        public void GetOldestMember_Returns_Oldest_Person()
        {
            // Arrange
            var oldestPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1950, 1, 1)
            };

            var youngPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var persons = new List<Person> { oldestPerson, youngPerson };

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetAllPerson()).Returns(persons);

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetOldestMember();

            // Assert
            Assert.Equal(oldestPerson, result);
        }

        [Fact]
        public void GetPersonById_Returns_Correct_Person()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var expectedPerson = new Person
            {
                Id = personId,
                FirstName = "John",
                LastName = "Doe"
            };

            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(repo => repo.GetPersonById(personId)).Returns(expectedPerson);

            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            var result = personService.GetPersonById(personId);

            // Assert
            Assert.Equal(expectedPerson, result);
        }

        [Fact]
        public void Update_Calls_Repository_Update_With_Correct_Person()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var updatedPerson = new Person
            {
                Id = personId,
                FirstName = "John",
                LastName = "Doe"
            };

            var mockPersonRepository = new Mock<IPersonRepository>();
            var personService = new PersonService(mockPersonRepository.Object);

            // Act
            personService.Update(updatedPerson);

            // Assert
            mockPersonRepository.Verify(repo => repo.UpdatePerson(updatedPerson), Times.Once);
        }
    }
}