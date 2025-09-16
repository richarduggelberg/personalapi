using Xunit;
using PersonalApi.Services;
using PersonalApi.Repositories;
using PersonalApi.Models;
using System.Linq;

namespace Tests.Services;

public class PersonServiceTests
{
    private PersonService _service;

    public PersonServiceTests()
    {
        _service = new PersonService(new PersonRepository());
    }

    [Fact]
    public void AddPerson_ShouldAddPerson()
    {
        var person = _service.AddPerson("John", "Doe", "john@example.com");
        var all = _service.GetAllPersons().ToList();

        Assert.Single(all);
        Assert.Equal("John", person.FirstName);
        Assert.Equal("Doe", person.LastName);
        Assert.Equal("john@example.com", person.Email);
    }

    [Fact]
    public void AddPerson_DuplicateEmail_ShouldThrow()
    {
        _service.AddPerson("John", "Doe", "john@example.com");

        Assert.Throws<ArgumentException>(() =>
            _service.AddPerson("Jane", "Smith", "john@example.com"));
    }

    [Fact]
    public void DeletePerson_ShouldRemovePerson()
    {
        _service.AddPerson("John", "Doe", "john@example.com");
        _service.DeletePersonByEmail("john@example.com");

        Assert.Empty(_service.GetAllPersons());
    }
}
