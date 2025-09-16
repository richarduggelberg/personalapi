# Personal API

A simple ASP.NET Core Web API for managing persons.

## Endpoints

- `POST /api/person?firstname=John&lastname=Doe&email=john@example.com`  
  Adds a new person. Returns 400 if inputs are invalid.

- `GET /api/person`  
  Returns a list of all persons.

- `DELETE /api/person/{email}`  
  Deletes a person by email. Returns 404 if not found.

## Input Validation

- First and last names: letters, hyphen, apostrophe only.
- Email: must be a valid email format.
- Email must be unique.

## Testing

- Unit tests are located in the `Tests` project.
- Property-based tests use FsCheck to validate inputs.


[Swagger UI (deployed)](https://personalapi-f3bcfedvhwdndxex.westeurope-01.azurewebsites.net/swagger/Index.html)
