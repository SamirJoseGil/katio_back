using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using katio.Business.Interfaces;


namespace katio.Business.Services;

public class NarratorService : INarratorService
{
    // Lista de narradores
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public NarratorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    

    // Traer todos los Narradores
    public async Task<BaseMessage<Narrator>> Index()
    {
        try
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<Narrator>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        } catch (Exception ex) 
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear Narradores
    public async Task<BaseMessage<Narrator>> CreateNarrator(Narrator narrator)
    {
        var existingNarrator = await _unitOfWork.NarratorRepository.GetAllAsync(n => n.Name == narrator.Name && n.LastName == narrator.LastName);

        if (existingNarrator.Any())
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.Conflict, $"{BaseMessageStatus.BAD_REQUEST_400} | El narrador ya está registrado en el sistema.");
        }
        var newNarrator = new Narrator()
        {
            Name = narrator.Name,
            LastName = narrator.LastName,
            Genre = narrator.Genre
        };
        try
        {
            await _unitOfWork.NarratorRepository.AddAsync(newNarrator);
            await _unitOfWork.SaveAsync();

        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { newNarrator });
    }


    // Actualizar Narradores
    public async Task<BaseMessage<Narrator>> UpdateNarrator(Narrator narrator)
    {
        var result= await _unitOfWork.NarratorRepository.FindAsync(narrator.Id);
        if (result == null)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.NotFound, BaseMessageStatus.NARRATOR_NOT_FOUND, new List<Narrator>());
        }
        result.Name = narrator.Name;
        result.LastName = narrator.LastName;
        result.Genre = narrator.Genre;
       
    
        try 
        {
            await _unitOfWork.NarratorRepository.AddAsync(result);
            await _unitOfWork.SaveAsync();

        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result });
    }
    // Eliminar Narradores
    public async Task<BaseMessage<Narrator>> DeleteNarrator(int id)
    {
        var result = await _unitOfWork.NarratorRepository.FindAsync(id);
        if (result == null)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.NotFound, BaseMessageStatus.NARRATOR_NOT_FOUND, new List<Narrator>());
        }
        try
        {
            await _unitOfWork.NarratorRepository.Delete(result);
            
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result });
    }
    #endregion

    #region  Find By Narrator
    //Buscar narrador por Id
    public async Task<BaseMessage<Narrator>> GetNarratorById(int id)
    {
        try
        {
            var result = await _unitOfWork.NarratorRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<Narrator>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Narrator> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    
    }

    // Buscar Narradores por Nombre
    public async Task<BaseMessage<Narrator>> GetNarratorsByName(string name)
    {
        try
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync(a => a.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Narrator>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.NARRATOR_NOT_FOUND, new List<Narrator>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
      
    // Buscar Narradores por Apellido  
    public async Task<BaseMessage<Narrator>> GetNarratorsByLastName(string lastName)
    {
        try
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync(b => b.LastName.ToLower().Contains(lastName.ToLower()));
            return result.Any()
                ? Utilities.BuildResponse<Narrator>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar Narradores por Genero
    public async Task<BaseMessage<Narrator>> GetNarratorsByGenre(string genre)
    {
        try
        {
            var result = await _unitOfWork.NarratorRepository.GetAllAsync(b => b.Genre.ToLower().Contains(genre.ToLower()));
            return result.Any()
                ? Utilities.BuildResponse<Narrator>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Narrator>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Narrator>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion

}
