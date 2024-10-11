using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudioBookController : ControllerBase
    {
        // Servicio de Audiolibros
        private readonly IAudioBookService _audioBookService;
        // Constructor
        public AudioBookController(IAudioBookService audioBookService)
        {
            _audioBookService = audioBookService;
        }

        // Trae todos los Audiolibros
        [HttpGet]
        [Route("GetAudioBooks")]
        public async Task<IActionResult> Index()
        {
            var response = await _audioBookService.Index();
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #region Create Update Delete

        // Crea un Audiolibro
        [HttpPost]
        [Route("CreateAudioBook")]
        public async Task<IActionResult> CreateAudioBook(AudioBook audioBook)
        {
            var response = await _audioBookService.CreateAudioBook(audioBook);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        // Actualiza un Audiolibro
        [HttpPut]
        [Route("UpdateAudioBook")]
        public async Task<IActionResult> UpdateAudioBook(AudioBook audioBook)
        {
            var response = await _audioBookService.UpdateAudioBook(audioBook);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Elimina un Audiolibro
        [HttpDelete]
        [Route("DeleteAudioBook")]
        public async Task<IActionResult> DeleteAudioBook(int id)
        {
            var response = await _audioBookService.DeleteAudioBook(id);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        #endregion

        #region Find By AudioBook

        // Busca AudioLibro por su Id
        [HttpGet]
        [Route("FindAudioBookById")]
        public async Task<IActionResult> GetByAudioBookId(int id)
        {
            var response = await _audioBookService.GetAudioBookById(id);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por su Nombre
        [HttpGet]
        [Route("FindAudioBookByName")]
        public async Task<IActionResult> GetByAudioBookName(string name)
        {
            var response = await _audioBookService.GetByAudioBookName(name);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por ISBN10
        [HttpGet]
        [Route("FindAudioBookByISBN10")]
        public async Task<IActionResult> GetByAudioBookISBN10(string isbn10)
        {
            var response = await _audioBookService.GetByAudioBookISBN10(isbn10);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por ISBN13
        [HttpGet]
        [Route("FindAudioBookByISBN13")]
        public async Task<IActionResult> GetAudioBookByISBN13(string isbn13)
        {
            var response = await _audioBookService.GetByAudioBookISBN13(isbn13);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por Rango de Publicacion
        [HttpGet]
        [Route("FindAudioBookByPublishedRange")]
        public async Task<IActionResult> GetAudioBookByPublishedRange(DateOnly startDate, DateOnly endDate)
        {
            var response = await _audioBookService.GetByAudioBookPublished(startDate, endDate);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por su Edicion
        [HttpGet]
        [Route("FindAudioBookByEdition")]
        public async Task<IActionResult> GetByAudioBookEdition(string edition)
        {
            var response = await _audioBookService.GetByAudioBookEdition(edition);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por su Genero
        [HttpGet]
        [Route("FindAudioBookByGenre")]
        public async Task<IActionResult> GetAudioBookByGenre(string genre)
        {
            var response = await _audioBookService.GetByAudioBookGenre(genre);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por su Duracion en Segundos
        [HttpGet]
        [Route("FindAudioBookByLenghtInSeconds")]
        public async Task<IActionResult> GetAudioBookByLenghtInSeconds(int lenghtInSeconds)
        {
            var response = await _audioBookService.GetByAudioBookLenghtInSeconds(lenghtInSeconds);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #endregion

        #region Find By Narrator

        // Busca un Audiolibro por su Narrador
        [HttpGet]
        [Route("FindAudioBookByNarrator")]
        public async Task<IActionResult> GetAudioBookByNarrator(int narratorId)
        {
            var response = await _audioBookService.GetAudioBookByNarrator(narratorId);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por nombre de su narrador
        [HttpGet]
        [Route("FindAudioBookByNarratorName")]
        public async Task<IActionResult> GetAudioBookByNarratorName(string narratorName)
        {
            var response = await _audioBookService.GetAudioBookByNarratorName(narratorName);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }
        
        // Busca un Audiolibro por apellido del narrador
        [HttpGet]
        [Route("FindAudioBookByNarratorLastName")]
        public async Task<IActionResult> GetAudioBookByNarratorLastName(string narratorLastName)
        {
            var response = await _audioBookService.GetAudioBookByNarratorLastName(narratorLastName);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Busca un Audiolibro por nombre completo de su narrador
        [HttpGet]
        [Route("FindAudioBookByNarratorFullName")]
        public async Task<IActionResult> GetAudioBookByNarratorFullName(string narratorName, string narratorLastName)
        {
            var response = await _audioBookService.GetAudioBookByNarratorFullName(narratorName, narratorLastName);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        [HttpGet]
        [Route("FindAudioBookByNarratorGenre")]
        public async Task<IActionResult> GetAudioBookByNarratorGenre(string genre)
        {
            var response = await _audioBookService.GetAudioBookByNarratorGenre(genre);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #endregion

    }
}
