using Assignment5.Application.Interfaces;
using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;
using Assignment5.WebApp.Areas.NashTech.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Assignment5.WebApp.Test.Areas.NashTech.Controllers
{
    public class RookiesControllerTests
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly RookiesController _controller;

        public RookiesControllerTests()
        {
            _mockPersonService = new Mock<IPersonService>();
            _controller = new RookiesController(_mockPersonService.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithAllPersons()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
            };
            _mockPersonService.Setup(service => service.GetAll()).Returns(persons.AsQueryable());

            // Act
            var result = _controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
            var model = result.Model as IQueryable<Person>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Create", result.ViewName);
        }

        [Fact]
        public void Create_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var person = new Person { };

            // Act
            var result = _controller.Create(person) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockPersonService.Verify(service => service.Create(person), Times.Once);
        }

        [Fact]
        public void Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FirstName", "Required");
            var person = new Person { };

            // Act
            var result = _controller.Create(person) as ViewResult;

            // Assert
            Assert.NotNull(result);
            _mockPersonService.Verify(service => service.Create(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void Create_AddsModelError_WhenExceptionOccurs()
        {
            // Arrange
            var person = new Person { };
            var exceptionMessage = "Test exception";
            _mockPersonService.Setup(service => service.Create(person)).Throws(new Exception(exceptionMessage));

            // Act
            var result = _controller.Create(person) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.ContainsKey(string.Empty));
            Assert.Equal($"Error{exceptionMessage}", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenServiceSucceeds()
        {
            // Arrange
            var personId = Guid.NewGuid();

            // Act
            var result = _controller.Delete(personId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockPersonService.Verify(service => service.Delete(personId), Times.Once);
        }

        [Fact]
        public void Delete_AddsModelError_WhenServiceThrowsException()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var errorMessage = "Service error message";
            _mockPersonService.Setup(service => service.Delete(personId)).Throws(new Exception(errorMessage));

            // Act
            var result = _controller.Delete(personId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.ContainsKey(string.Empty));
            var modelStateError = _controller.ModelState[string.Empty].Errors[0];
            Assert.Equal("Error" + errorMessage, modelStateError.ErrorMessage);
        }

        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenIdIsNull()
        {
            // Arrange
            Guid? id = null;

            // Act
            var result = _controller.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenPersonNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            _mockPersonService.Setup(service => service.GetPersonById(id)).Returns((Person)null);

            // Act
            var result = _controller.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_ReturnsViewResult_WhenPersonFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var person = new Person { Id = id, };
            _mockPersonService.Setup(service => service.GetPersonById(id)).Returns(person);

            // Act
            var result = _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person, result.Model);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithPerson_WhenPersonExists()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, };
            _mockPersonService.Setup(service => service.GetPersonById(personId)).Returns(person);

            // Act
            var result = _controller.Details(personId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person, result.Model);
        }

        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenPersonIsNull()
        {
            // Arrange
            var nonExistingPersonId = Guid.NewGuid();
            _mockPersonService.Setup(service => service.GetPersonById(nonExistingPersonId)).Returns((Person)null);

            // Act
            var result = _controller.Details(nonExistingPersonId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenIdIsNull()
        {
            // Act
            var result = _controller.Details(null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GetMaleMember_ReturnsViewResult_WithListOfPersons()
        {
            // Arrange
            var malePersons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), Gender = GenderType.Male,},
                new Person { Id = Guid.NewGuid(), Gender = GenderType.Male, },
                new Person { Id = Guid.NewGuid(), Gender = GenderType.Male,}
            };
            _mockPersonService.Setup(service => service.GetMaleMembers()).Returns(malePersons);

            // Act
            var result = _controller.GetMaleMember() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as List<Person>;
            Assert.NotNull(model);
            Assert.Equal(malePersons.Count, model.Count);
        }

        [Fact]
        public void GetMaleMember_ReturnsRedirectToActionResult_WhenListIsNull()
        {
            // Arrange
            _mockPersonService.Setup(service => service.GetMaleMembers()).Returns((List<Person>)null);

            // Act
            var result = _controller.GetMaleMember() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GetOldestMember_ReturnsViewResult_WithOldestPerson_WhenOldestPersonExists()
        {
            // Arrange
            var oldestPerson = new Person { Id = Guid.NewGuid(), };
            _mockPersonService.Setup(service => service.GetOldestMember()).Returns(oldestPerson);

            // Act
            var result = _controller.GetOldestMember() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Details", result.ViewName);
            var model = result.Model as Person;
            Assert.NotNull(model);
            Assert.Equal(oldestPerson.Id, model.Id);
        }

        [Fact]
        public void GetOldestMember_ReturnsRedirectToActionResult_WhenOldestPersonIsNull()
        {
            // Arrange
            _mockPersonService.Setup(service => service.GetOldestMember()).Returns((Person)null);

            // Act
            var result = _controller.GetOldestMember() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GetMemberFullNames_ReturnsViewResult_WithMemberFullNames_WhenMemberFullNamesExist()
        {
            // Arrange
            var memberFullNames = new List<string> { "John Doe", "Jane Smith", "Michael Johnson" };
            _mockPersonService.Setup(service => service.GetMemberFullNames()).Returns(memberFullNames);

            // Act
            var result = _controller.GetMemberFullNames() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("FullNames", result.ViewName);
            var model = result.Model as List<string>;
            Assert.NotNull(model);
            Assert.Equal(memberFullNames.Count, model.Count);
        }

        [Fact]
        public void GetMemberFullNames_ReturnsRedirectToActionResult_WhenMemberFullNamesAreNull()
        {
            // Arrange
            _mockPersonService.Setup(service => service.GetMemberFullNames()).Returns((List<string>)null);

            // Act
            var result = _controller.GetMemberFullNames() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsViewResult_WithFilteredPersons_WhenPersonsExist()
        {
            // Arrange
            var filteredPersons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(),  },
                new Person { Id = Guid.NewGuid(),  },
                new Person { Id = Guid.NewGuid(), }
            };
            _mockPersonService.Setup(service => service.FilterMembersByBirthYear(It.IsAny<int>(), It.IsAny<string>())).Returns(filteredPersons);

            // Act
            var result = _controller.FilterMembersByBirthYear(1990, "greaterThan") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
            var model = result.Model as List<Person>;
            Assert.NotNull(model);
            Assert.Equal(filteredPersons.Count, model.Count);
        }

        [Fact]
        public void FilterMembersByBirthYear_ReturnsRedirectToActionResult_WhenPersonsAreNull()
        {
            // Arrange
            _mockPersonService.Setup(service => service.FilterMembersByBirthYear(It.IsAny<int>(), It.IsAny<string>())).Returns((List<Person>)null);

            // Act
            var result = _controller.FilterMembersByBirthYear(1990, "greaterThan") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void ExportToExcel_ReturnsFileResult_WhenFileIsNotNull()
        {
            // Arrange
            byte[] excelFile = new byte[] { };
            _mockPersonService.Setup(service => service.GetMembersAsExcelFile()).Returns(excelFile);

            // Act
            var result = _controller.ExportToExcel() as FileResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.ContentType);
            Assert.Equal("Members.xlsx", result.FileDownloadName);
            Assert.Equal(excelFile, (result as FileContentResult)?.FileContents);
        }

        [Fact]
        public void ExportToExcel_ReturnsRedirectToActionResult_WhenFileIsNull()
        {
            // Arrange
            _mockPersonService.Setup(service => service.GetMembersAsExcelFile()).Returns((byte[])null);

            // Act
            var result = _controller.ExportToExcel() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FirstName", "Required");
            var invalidPerson = new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

            // Act
            var result = _controller.Edit(invalidPerson) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invalidPerson, result.Model);
        }

        [Fact]
        public void Edit_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var validPerson = new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

            // Act
            var result = _controller.Edit(validPerson) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_AddsModelError_WhenUpdateThrowsException()
        {
            // Arrange
            var validPerson = new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };
            var errorMessage = "Error updating person";
            _mockPersonService.Setup(repo => repo.Update(It.IsAny<Person>())).Throws(new Exception(errorMessage));

            // Act
            var result = _controller.Edit(validPerson) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.ContainsKey(string.Empty));
            Assert.Contains(errorMessage, _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }
    }
}