using Xunit;
using PersonalApi.Services;
using PersonalApi.Repositories;
using PersonalApi.Models;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;

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
    public void AddPerson_TwoDifferentEmails_ShouldWork()
    {
        var person1 = _service.AddPerson("John", "Doe", "john@example.com");
        var person2 = _service.AddPerson("Jane", "Smith", "jane@example.com");

        var all = _service.GetAllPersons().ToList();

        Assert.Equal(2, all.Count);
        Assert.Contains(all, p => p.Email == "john@example.com" && p.FirstName == "John" && p.LastName == "Doe");
        Assert.Contains(all, p => p.Email == "jane@example.com" && p.FirstName == "Jane" && p.LastName == "Smith");
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

    [Fact]
    public void AddPerson_EmptyFirstName_ShouldThrow()
    {
        var service = new PersonService(new PersonRepository());

        Assert.Throws<ArgumentException>(() =>
            service.AddPerson("", "Doe", "john@example.com"));
    }

    [Fact]
    public void AddPerson_EmptyLastName_ShouldThrow()
    {
        var service = new PersonService(new PersonRepository());

        Assert.Throws<ArgumentException>(() =>
            service.AddPerson("John", "", "john@example.com"));
    }

    [Fact]
    public void AddPerson_EmptyEmail_ShouldThrow()
    {
        var service = new PersonService(new PersonRepository());

        Assert.Throws<ArgumentException>(() =>
            service.AddPerson("John", "Doe", ""));
    }

    // Helper to filter out control characters
    public static class CustomGenerators
    {
        // Define what is allowed in names (letters, spaces, hyphens, maybe apostrophes)
        private static bool IsValidNameChar(char c) =>
            char.IsLetter(c) || c == ' ' || c == '-' || c == '\'';

        // Define what is allowed in emails (basic version)
        private static bool IsValidEmailChar(char c) =>
            char.IsLetterOrDigit(c) || c == '@' || c == '.' || c == '-' || c == '_';

        public static Arbitrary<NonEmptyString> ValidName()
        {
            return Arb.Default.NonEmptyString()
                    .Filter(s => s.Get.All(IsValidNameChar) && !s.Get.Any(char.IsWhiteSpace));
        }

        public static Arbitrary<NonEmptyString> ValidEmail()
        {
            return Arb.Default.NonEmptyString()
                    .Filter(s => s.Get.All(IsValidEmailChar) && !s.Get.Any(char.IsWhiteSpace));
        }
    }

    [Property(Arbitrary = new[] { typeof(CustomGenerators) })]
    public void AddPerson_WithArbitraryInputs_ShouldBeRetrievable(
        NonEmptyString firstName,
        NonEmptyString lastName,
        NonEmptyString email)
    {
        var service = new PersonService(new PersonRepository());

        // Ensure unique email (so it won’t collide with itself)
        var safeEmail = email.Item + Guid.NewGuid() + "@example.com";

        var person = service.AddPerson(firstName.Item, lastName.Item, safeEmail);

        var all = service.GetAllPersons().ToList();

        Assert.Contains(all, p =>
            p.FirstName == firstName.Item &&
            p.LastName == lastName.Item &&
            p.Email == safeEmail
        );
    }
}
