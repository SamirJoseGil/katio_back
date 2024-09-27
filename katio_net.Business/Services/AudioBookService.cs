﻿using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace katio.Business.Services;

public class AudioBookService : IAudioBookService
{
    // Lista de libros
    private readonly KatioContext _context;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public AudioBookService(KatioContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    // Constructor

    // Traer todos los Audiolibros
    public async Task<BaseMessage<AudioBook>> Index()
    {
        // var result = await _context.AudioBooks.ToListAsync();
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync();
        return result.Any() ? Utilities.BuildResponse<AudioBook>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    #region Create Update Delete
    // Crear un Audiolibro
    public async Task<BaseMessage<AudioBook>> CreateAudioBook(AudioBook audioBook)
    {
        var existingAudiobook = await  _unitOfWork.AudioBookRepository.GetAllAsync(
        a => a.ISBN10 == audioBook.ISBN10 || a.ISBN13 == audioBook.ISBN13);

        if (existingAudiobook.Any())
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.Conflict, "El audiolibro ya existe en el sistema con el mismo ISBN10 o ISBN13.", existingAudiobook);
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
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<AudioBook>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { newAudioBook });
    }
    
    // Actualizar un Audiolibro
    public async Task<BaseMessage<AudioBook>> UpdateAudioBook(AudioBook audioBook)
    {
        //var result = _context.AudioBooks.FirstOrDefault(b => b.Id == audioBook.Id);
        var result = await _unitOfWork.AudioBookRepository.FindAsync(audioBook.Id);
        if (result == null)
        {
            return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<AudioBook>());
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
        
        await _unitOfWork.AudioBookRepository.Update(result);
        await _unitOfWork.SaveAsync();
        
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result });
    }

    // Eliminar un Audiolibro
    public async Task<BaseMessage<AudioBook>> DeleteAudioBook(int id)
    {
        // var result = _context.AudioBooks.FirstOrDefault(b => b.Id == Id);
        var result = await _unitOfWork.AudioBookRepository.FindAsync(id);

        if (result != null)
        {
            await _unitOfWork.AudioBookRepository.Delete(result);
            await _unitOfWork.SaveAsync();
            return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result }  );
        }
    
    return Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }
    #endregion

    #region Find By AudioBook
    // Buscar por id
    public async Task<BaseMessage<AudioBook>> GetAudioBookById(int id)
    {
        var result = await _unitOfWork.AudioBookRepository.FindAsync(id);
        return result != null ? Utilities.BuildResponse<AudioBook>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<AudioBook> { result }) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    } 
    // Buscar por Nombre
    public async Task<BaseMessage<AudioBook>> GetByAudioBookName(string name)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por ISBN10
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN10(string ISBN10)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.ISBN10 == ISBN10);
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por ISBN13
    public async Task<BaseMessage<AudioBook>> GetByAudioBookISBN13(string ISBN13)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.ISBN13 == ISBN13);
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Rango de Fecha de Publicación
    public async Task<BaseMessage<AudioBook>> GetByAudioBookPublished(DateOnly startDate, DateOnly endDate)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Edición
    public async Task<BaseMessage<AudioBook>> GetByAudioBookEdition(string edition)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Edition.Contains(edition, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Género
    public async Task<BaseMessage<AudioBook>> GetByAudioBookGenre(string genre)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase));
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Duración en Segundos
    public async Task<BaseMessage<AudioBook>> GetByAudioBookLenghtInSeconds(int lenghtInSeconds)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.LenghtInSeconds == lenghtInSeconds);
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    #endregion

    #region Find By Author

    // Buscar por Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthor(int authorId)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.AuthorId == authorId, includeProperties: "Author");
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Nombre de Autor   
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorName(string authorName)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase), includeProperties: "Author");
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Apellido del Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorLastName(string authorLastName)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase), includeProperties: "Author");
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Nombre y Apellido de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorFullName(string authorName, string authorLastName)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(
            b => b.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase) &&
                b.Author.LastName.Contains(authorLastName, StringComparison.InvariantCultureIgnoreCase), 
            includeProperties: "Author");
        
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por País de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorCountry(string authorCountry)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.Country.Contains(authorCountry, StringComparison.InvariantCultureIgnoreCase), includeProperties: "Author");
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }

    // Buscar por Rango de Fecha de Nacimiento de Autor
    public async Task<BaseMessage<AudioBook>> GetAudioBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        var result = await _unitOfWork.AudioBookRepository.GetAllAsync(b => b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate, includeProperties: "Author");
        return result.Any() ? 
            Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, result) : 
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.AUDIOBOOK_NOT_FOUND, new List<AudioBook>());
    }


    #endregion
}
