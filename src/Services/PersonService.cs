using PersonalApi.Models;
using PersonalApi.Repositories;

namespace PersonalApi.Services;

public interface IPersonService
{
    Person AddPerson(string firstname, string lastname, string email);
    IEnumerable<Person> GetAllPersons();
}

public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public Person AddPerson(string firstname, string lastname, string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Invalid email");

        var existingPersons = GetAllPersons();

        if (existingPersons.Contains(p => p.Email.Equals(email))) {
            throw new ArgumentException("Person with the email already exists");
        }

        var newPerson = new Person
        {
            FirstName = firstname,
            LastName = lastname,
            Email = email
        };

        _repository.Add(newPerson);

        return newPerson;
    }

    public IEnumerable<Person> GetAllPersons()
    {
        return _repository.GetAll();
    }
}
