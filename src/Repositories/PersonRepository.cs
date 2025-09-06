using PersonalApi.Models;

namespace PersonalApi.Repositories;

public interface IPersonRepository
{
    void Add(Person person);
    IEnumerable<Person> GetAll();
    void Remove(Person person);
}

public class PersonRepository : IPersonRepository
{
    private readonly List<Person> _persons = new();

    public void Add(Person person) => _persons.Add(person);

    public IEnumerable<Person> GetAll() => _persons;

    public void Remove(Person person) => _persons.Remove(person);
}

