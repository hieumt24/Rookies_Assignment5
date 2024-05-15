using Assignment5.Domain.Entities;
using Assignment5.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Infrastructure.Tests
{
    public class MyDbContextTests
    {
        [Fact]
        public void PersonList_Returns_ListOfPersons()
        {
            // Arrange
            var expectedCount = 10;

            // Act
            var persons = MyDbContext.PersonList();

            // Assert
            Assert.NotNull(persons);
            Assert.Equal(expectedCount, persons.Count);
        }
    }
}