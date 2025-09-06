using Microsoft.AspNetCore.Mvc;
using PersonalApi.Models;
using PersonalApi.Services;

namespace PersonalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{

    private readonly IPersonService _service;
    
    public PersonController(IPersonService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create([FromQuery] string firstname, [FromQuery] string lastname, [FromQuery] string email)
    {
        try
        {
            var person = _service.AddPerson(firstname, lastname, email);
            return Ok(new { message = "person added", person });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var persons = _service.GetAllPersons();
        return Ok(persons);
    }
}

