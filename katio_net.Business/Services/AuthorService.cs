using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;

namespace katio.Business.Services;

public class AuthorService : IAuthorService
{
    // Lista de autores
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AuthorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los Autores
    public async Task<BaseMessage<Author>> Index()
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync();
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }


    #region Create Update Delete

    // Crear Autores
    public async Task<BaseMessage<Author>> CreateAuthor(Author author)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Name == author.Name && a.LastName == author.LastName);

        if (existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.Conflict, BaseMessageStatus.AUTHOR_ALREADY_EXISTS);
        }
        try
        {
            await _unitOfWork.AuthorRepository.AddAsync(author);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author });
    }

    // Actualizar Autores
    public async Task<BaseMessage<Author>> UpdateAuthor(Author author)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Name == author.Name && a.LastName == author.LastName);

        if (!existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AuthorRepository.Update(author);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author });
    }

    // Eliminar Autores
    public async Task<BaseMessage<Author>> DeleteAuthor(int id)
    {
        var existingAuthor = await _unitOfWork.AuthorRepository.GetAllAsync(a => a.Id == id);

        if (!existingAuthor.Any())
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.Conflict, BaseMessageStatus.AUTHOR_NOT_FOUND);
        }
        try
        {
            await _unitOfWork.AuthorRepository.Delete(id);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }

        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { });
    }

    #endregion

    #region Find By Author
    //Traer autores por Id
    public async Task<BaseMessage<Author>> GetAuthorById(int id)
    {
        try
        {
            var author = await _unitOfWork.AuthorRepository.FindAsync(id);
            return author != null ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Author> { author }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer los autores por nombre
    public async Task<BaseMessage<Author>> GetAuthorsByName(string name)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por apellido
    //este metodo
    public async Task<BaseMessage<Author>> GetAuthorsByLastName(string LastName)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.LastName.ToLower().Contains(LastName.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por pais - region
    public async Task<BaseMessage<Author>> GetAuthorsByCountry(string Country)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.Country.ToLower().Contains(Country.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    // Traer los autores por rango de fecha de nacimiento
    public async Task<BaseMessage<Author>> GetAuthorsByBirthDate(DateOnly StartDate, DateOnly EndDate)
    {
        try
        {
            var result = await _unitOfWork.AuthorRepository.GetAllAsync(b => b.BirthDate >= StartDate && b.BirthDate <= EndDate);
            return result.Any() ? Utilities.BuildResponse<Author>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUTHOR_NOT_FOUND, new List<Author>());
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Author>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}
