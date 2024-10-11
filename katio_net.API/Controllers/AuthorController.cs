using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        // Servicio de autores
        private readonly IAuthorService _authorService;

        // Constructor
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // Trae todos los autores
        [HttpGet]
        [Route("GetAuthors")]
        public async Task<IActionResult> Index()
        {
            var response = await _authorService.Index();
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #region Create Update Delete

        // Crear Autores
        [HttpPost]
        [Route("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            var response = await _authorService.CreateAuthor(author);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        // Actualizar Autores
        [HttpPut]
        [Route("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(Author author)
        {
            var response = await _authorService.UpdateAuthor(author);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Elimina un Autor
        [HttpDelete]
        [Route("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var response = await _authorService.DeleteAuthor(id);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        #endregion

        #region Find By Author

        // Trae un Autor por su Id
        [HttpGet]
        [Route("GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(int Id)
        {
            var author = await _authorService.GetAuthorById(Id);
            return author != null ? Ok(author) : StatusCode(StatusCodes.Status404NotFound, author);
        }

        // Trae un autor por su nombre
        [HttpGet]
        [Route("GetAuthorByName")]
        public async Task<IActionResult> GetAuthorByName(string name)
        {
            var response = await _authorService.GetAuthorsByName(name);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un autor por su apellido
        [HttpGet]
        [Route("GetAuthorByLastName")]
        public async Task<IActionResult> GetAuthorByLastName(string lastName)
        {
            var response = await _authorService.GetAuthorsByLastName(lastName);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un autor por su pais - region
        [HttpGet]
        [Route("GetAuthorByCountry")]
        public async Task<IActionResult> GetAuthorByCountry(string country)
        {
            var response = await _authorService.GetAuthorsByCountry(country);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un autor por rango de fecha
        [HttpGet]
        [Route("GetAuthorByBirthDate")]
        public async Task<IActionResult> GetAuthorByBirthDate(DateOnly startDate, DateOnly endDate)
        {
            var response = await _authorService.GetAuthorsByBirthDate(startDate, endDate);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #endregion
    }
}
