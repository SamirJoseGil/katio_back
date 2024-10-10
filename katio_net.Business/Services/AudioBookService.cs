using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

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
    // Constructor

    // Traer todos los Audiolibros
    public async Task<BaseMessage<AudioBook>> Index()
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex) {
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
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.Conflict, $"{BaseMessageStatus.BAD_REQUEST_400} |  Ya hay un audiolibro registrado con el mismo ISBN.");
        }

        var newAudioBook = new AudioBook()
        {
            Name = audioBook.Name,
            ISBN10 = audioBook.ISBN10,
            ISBN13 = audioBook.ISBN13,
            Published = audioBook.Published,
            Edition = audioBook.Edition,
            Genre = audioBook.Genre,
            LenghtInSeconds = audioBook.LenghtInSeconds,
            Path = audioBook.Path,
            AuthorId = audioBook.AuthorId
        };
        try
        {
            await _unitOfWork.AudioBookRepository.AddAsync(newAudioBook);
            await _unitOfWork.SaveAsync();

        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { newAudioBook });
    }
    
    // Actualizar un Audiolibro
    public async Task<BaseMessage<AudioBook>> UpdateAudioBook(AudioBook audioBook)
    {
        var result= await _unitOfWork.AudioBookRepository.FindAsync(audioBook.Id);
        if (result == null)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        
        result.Name = audioBook.Name;
        result.ISBN10 = audioBook.ISBN10;
        result.ISBN13 = audioBook.ISBN13;
        result.Published = audioBook.Published;
        result.Edition = audioBook.Edition;
        result.Genre = audioBook.Genre;
        result.LenghtInSeconds = audioBook.LenghtInSeconds;
        result.Path = audioBook.Path;
        result.AuthorId = audioBook.AuthorId;
       
        try 
        {
            await _unitOfWork.AudioBookRepository.Update(result);
            await _unitOfWork.SaveAsync();
            
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> {result});
    }

    // Eliminar un Audiolibro
    public async Task<BaseMessage<AudioBook>> DeleteAudioBook(int Id)
    {
        var result = await _unitOfWork.AudioBookRepository.FindAsync(Id);
        if (result == null)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        }
        try
        {
            await _unitOfWork.AudioBookRepository.Delete(result);
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result });
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
        } catch (Exception ex)
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
       } catch (Exception ex)
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
        } catch (Exception ex)
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
        } catch (Exception ex)
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
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Edición
    public async Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition)
    {
        try {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Edition.ToLower().Contains( edition.ToLower()) );
        return result.Any() ? Utilities.BuildResponse<AudioBook>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
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
        } catch (Exception ex)
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
            return (result != null)
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result })
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    #endregion

    #region Find By Author

    // Buscar por Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthor(int authorId)
    {
        try 
        {
            var result = await _unitOfWork.AudioBookRepository.FindAsync(authorId);
            return (result != null)
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result })
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por Nombre de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorName(string authorName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Author.Name.ToLower().Contains(authorName.ToLower()),
            includeProperties: "Author");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Apellido del Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorLastName(string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.LastName.ToLower().Contains(authorLastName.ToLower()),
            includeProperties: "Author");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex) {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Nombre y Apellido de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorFullName(string authorName, string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Author.Name.ToLower().Contains(authorName.ToLower()) &&
            a.Author.LastName.ToLower().Contains(authorLastName.ToLower()), 
            includeProperties: "Author");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    // Buscar por País de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorCountry(string authorCountry)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(a => a.Author.Country.ToLower().Contains(authorCountry.ToLower()), 
            includeProperties: "Author");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Buscar por Rango de Fecha de Nacimiento de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate,
            includeProperties: "Author");
            return result.Any()
                ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result)
                : Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}
