using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;

namespace katio.Business.Services;

public class AudioBookService : IAudioBookService
{
    // Lista de libros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AudioBookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los Audiolibros
    public async Task<BaseMessage<AudioBook>> Index()
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #region Create Update Delete

    // Crear un Audiolibro
    public async Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.ISBN10 == audioBook.ISBN10 || ab.ISBN13 == audioBook.ISBN13);

        if (existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.Conflict, BaseMessageStatus.AUDIOBOOK_ALREADY_EXISTS);
        }
        try
        {
            await _unitOfWork.AudioBookRepository.AddAsync(audioBook);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { audioBook });
    }

    // Actualizar un Audiolibro
    public async Task<BaseMessage<AudioBook>> UpdateAudioBook(AudioBook audioBook)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.ISBN10 == audioBook.ISBN10 || ab.ISBN13 == audioBook.ISBN13);

        if (!existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AudioBookRepository.Update(audioBook);
            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { audioBook });
    }

    // Eliminar un Audiolibro
    public async Task<BaseMessage<AudioBook>> DeleteAudioBook(int id)
    {
        var existingAudioBook = await _unitOfWork.AudioBookRepository.GetAllAsync(ab => ab.Id == id);

        if (!existingAudioBook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        try
        {
            await _unitOfWork.AudioBookRepository.Delete(id);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { });
    }
    #endregion

    #region Find By AudioBook
    // Buscar por id
    public async Task<BaseMessage<AudioBook>> GetAudioBookById(int id)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<AudioBook>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Buscar por Nombre
    public async Task<BaseMessage<AudioBook>> GetByAudioBookName(string name)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por ISBN10
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.ISBN10 == ISBN10);
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por ISBN13
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.ISBN13 == ISBN13);
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Rango de Fecha de Publicación
    public async Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
            return result.Any() ? Utilities.BuildResponse<AudioBook>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Edición
    public async Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Edition.ToLower().Contains(edition.ToLower()));
            return result.Any() ? Utilities.BuildResponse<AudioBook>
                    (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                    Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Género
    public async Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Genre.ToLower().Contains(genre.ToLower()));
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por Duración en Segundos
    public async Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(lenghtInSeconds);
            return result != null
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result })
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    #endregion

    #region Find By Narrator

    // Buscar por Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarrator(int narratorId)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(narratorId);
            return (result != null)
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result })
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por Nombre de Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorName(string narratorName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Narrator.Name.ToLower().Contains(narratorName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Apellido del Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorLastName(string narratorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Narrator.LastName.ToLower().Contains(narratorLastName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Nombre y Apellido de Narrador
    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorFullName(string narratorName, string narratorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Narrator.Name.ToLower().Contains(narratorName.ToLower()) &&
            a.Narrator.LastName.ToLower().Contains(narratorLastName.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    public async Task<BaseMessage<AudioBook>> GetAudioBookByNarratorGenre(string genre)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Narrator.Genre.ToLower().Contains(genre.ToLower()),
            includeProperties: "Narrator");
            return result.Any()
                    ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                    : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}