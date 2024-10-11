using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;

namespace katio.Business.Services;

public class GenreService : IGenreService
{
    // Lista de géneros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public GenreService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los géneros
    public async Task<BaseMessage<Genre>> Index()
    {
        try
        {
            var result = await _unitOfWork.GenreRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<Genre>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.GENRE_NOT_FOUND, new List<Genre>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear géneros
    public async Task<BaseMessage<Genre>> CreateGenre(Genre genre)
    {
        var existingGenre = await _unitOfWork.GenreRepository.GetAllAsync(g => g.Name == genre.Name);

        if (existingGenre.Any())
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.Conflict, $"{BaseMessageStatus.BAD_REQUEST_400} | El género ya está registrado en el sistema.");
        }
        var newGenre = new Genre()
        {
            Name = genre.Name,
            Description = genre.Description
        };
        try
        {
            await _unitOfWork.GenreRepository.AddAsync(newGenre);
            await _unitOfWork.SaveAsync();
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Genre> { newGenre });
    }

    // Actualizar géneros
    public async Task<BaseMessage<Genre>> UpdateGenre(Genre genre)
    {
        var result = await _unitOfWork.GenreRepository.FindAsync(genre.Id);
        if (result == null)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.NotFound, BaseMessageStatus.GENRE_NOT_FOUND);
        }
        
        result.Name = genre.Name;
        result.Description = genre.Description;

        try
        {
            await _unitOfWork.GenreRepository.Update(result);
            await _unitOfWork.SaveAsync();
            
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }

        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Genre> { result });
    }

    // Eliminar géneros
    public async Task<BaseMessage<Genre>> DeleteGenre(int id)
    {
        var result = await _unitOfWork.GenreRepository.FindAsync(id);
        if (result == null)
        {
            return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.GENRE_NOT_FOUND, new List<Genre>());
        }
        try
        {
            await _unitOfWork.GenreRepository.Delete(result);
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Genre> { result });
    }

    #endregion

    #region Find By Genre
    // Buscar por el id
    public async Task<BaseMessage<Genre>> GetByGenreId(int id)
    {
        try
        {
            var result = await _unitOfWork.GenreRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<Genre>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Genre> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.GENRE_NOT_FOUND, new List<Genre>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar género por Nombre
    public async Task<BaseMessage<Genre>> GetGenresByName(string name)
    {
        try
        {
            var result = await _unitOfWork.GenreRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Genre>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Genre>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    //Buscar genero por descripcion
    public async Task<BaseMessage<Genre>> GetGenresByDescription(string description)
    {
        try
        {
            var result = await _unitOfWork.GenreRepository.GetAllAsync(b => b.Description.ToLower().Contains(description.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Genre>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Genre>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Genre>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}
