using Microsoft.AspNetCore.Mvc;
using katio.Business.Interfaces;
using katio.Data.Models;

namespace katio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NarratorController : ControllerBase
    {
        // Servicio de narradores
        private readonly INarratorService _narratorService;

        // Constructor
        public NarratorController(INarratorService narratorService)
        {
            _narratorService = narratorService;
        }

        // Trae todos los narradores
        [HttpGet]
        [Route("GetNarrators")]
        public async Task<IActionResult> Index()
        {
            var response = await _narratorService.Index();
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #region Create Update Delete

        // Crear Narradores
        [HttpPost]
        [Route("CreateNarrator")]
        public async Task<IActionResult> CreateNarrator(Narrator narrator)
        {
            var response = await _narratorService.CreateNarrator(narrator);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        // Actualizar Narradores
        [HttpPut]
        [Route("UpdateNarrator")]
        public async Task<IActionResult> UpdateNarrator(Narrator narrator)
        {
            var response = await _narratorService.UpdateNarrator(narrator);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Elimina un Narrador
        [HttpDelete]
        [Route("DeleteNarrator")]
        public async Task<IActionResult> DeleteNarrator(int id)
        {
            var response = await _narratorService.DeleteNarrator(id);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }

        #endregion

        #region Find By Narrator

        // Trae un narrador por su id
        [HttpGet]
        [Route("GetNarratorById")]
        public async Task<IActionResult> GetNarratorById(int Id)
        {
            var response = await _narratorService.GetNarratorById(Id);
            return response != null ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un narrador por su Nombre
        [HttpGet]
        [Route("GetNarratorByName")]
        public async Task<IActionResult> GetNarratorByName(string name)
        {
            var response = await _narratorService.GetNarratorsByName(name);
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un narrador por su Apellido
        [HttpGet]
        [Route("GetNarratorByLastName")]
        public async Task<IActionResult> GetNarratorByLastName(string lastName)
        {
            var response = await _narratorService.GetNarratorsByLastName(lastName);
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        // Trae un narrador por su Genero
        [HttpGet]
        [Route("GetNarratorByGenre")]
        public async Task<IActionResult> GetNarratorByGenre(string genre)
        {
            var response = await _narratorService.GetNarratorsByGenre(genre);
            return response.TotalElements > 0 ? Ok(response) : StatusCode(StatusCodes.Status404NotFound, response);
        }

        #endregion 
    }
}
