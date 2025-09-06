using PersonalApi.Models;
using PersonalApi.Repositories;

namespace PersonalApi.Services;

public interface IPersonService
{
    Person AddPerson(string firstname, string lastname, string email);
    IEnumerable<Person> GetAllPersons();
    void DeletePersonByEmail(string email);
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
        if (string.IsNullOrWhiteSpace(firstname) ||
            string.IsNullOrWhiteSpace(lastname) ||
            string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Firstname, lastname, and email cannot be empty");
        }

        var existingPersons = GetAllPersons();

        if (existingPersons.Any(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Person with this email already exists");
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

    public void DeletePersonByEmail(string email)
    {
        var person = _repository.GetAll()
            .FirstOrDefault(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (person == null)
            throw new ArgumentException("Person not found");

        _repository.Remove(person); // We'll need to add this method in the repository
    }
}
