using katio.Business.Interfaces;
using katio.Data.Models;
using katio.Data.Dto;
using katio.Data;
using System.Net;

namespace katio.Business.Services;

public class BookService : IBookService
{
    // Lista de libros
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Traer todos los libros
    public async Task<BaseMessage<Book>> Index()
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync();
            return Utilities.BuildResponse<Book>(HttpStatusCode.OK, BaseMessageStatus.OK_200, result);
        }
        catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, BaseMessageStatus.INTERNAL_SERVER_ERROR_500);
        }
    }

    #region Create Update Delete

    // Crear un Libro
    public async Task<BaseMessage<Book>> CreateBook(Book book)
    {
        var existingBook = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == book.ISBN10 || b.ISBN13 == book.ISBN13);

        if (existingBook.Any())
        {
           
            return Utilities.BuildResponse<Book>(HttpStatusCode.Conflict, $"{BaseMessageStatus.BAD_REQUEST_400} | Ya hay un libro registrado con el mismo ISBN.");
        }
        var newBook = new Book()
        {
            Name = book.Name,
            ISBN10 = book.ISBN10,
            ISBN13 = book.ISBN13,
            Published = book.Published,
            Edition = book.Edition,
            DeweyIndex = book.DeweyIndex,
            AuthorId = book.AuthorId
        };
        try
        {
            await _unitOfWork.BookRepository.AddAsync(newBook);
            await _unitOfWork.SaveAsync();

        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { newBook });
    }

    // Actualizar un Libro
    public async Task<BaseMessage<Book>> UpdateBook(Book book)
    {
        var result= await _unitOfWork.BookRepository.FindAsync(book.Id);
        if (result == null)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        result.Name = book.Name;
        result.ISBN10 = book.ISBN10;
        result.ISBN13 = book.ISBN13;
        result.Published = book.Published;
        result.Edition = book.Edition;
        result.DeweyIndex = book.DeweyIndex;

        try 
        {
            await _unitOfWork.BookRepository.Update(result);
            await _unitOfWork.SaveAsync();
            
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result });
    }

    // Eliminar un libro
    public async Task<BaseMessage<Book>> DeleteBook(int id)
    {
        var result = await _unitOfWork.BookRepository.FindAsync(id);
        if (result == null)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        }
        try
        {
            await _unitOfWork.BookRepository.Delete(result);
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
        return Utilities.BuildResponse(HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result });
    }

    #endregion

    #region Find By Book
    // Traer libros por id
    public async Task<BaseMessage<Book>> GetBookById(int id)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.FindAsync(id);
            return result != null ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, new List<Book> { result }) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre
    public async Task<BaseMessage<Book>> GetBooksByName(string name)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Name.ToLower().Contains(name.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por ISBN10
    public async Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10)
    {
        try
        {
        var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN10 == ISBN10);
        return result.Any() ? Utilities.BuildResponse<Book>
            (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
            Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por ISBN13
    public async Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.ISBN13 == ISBN13);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por rango de fecha de publicación
    public async Task<BaseMessage<Book>> GetBooksByPublished(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Published >= startDate && b.Published <= endDate);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex) 
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por edición
    public async Task<BaseMessage<Book>> GetBooksByEdition(string edition)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.Edition.ToLower().Contains(edition.ToLower()));
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por índice Dewey
    public async Task<BaseMessage<Book>> GetBooksByDeweyIndex(string deweyIndex)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(b => b.DeweyIndex == deweyIndex);
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    #endregion

    #region Find By Author

    // Traer libros por autor
    public async Task<BaseMessage<Book>> GetBookByAuthorAsync(int authorId)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                ((b => b.AuthorId == authorId),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorNameAsync(string authorName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                (b => b.Author.Name.ToLower().Contains(authorName.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorLastNameAsync(string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync
                (b => b.Author.LastName.ToLower().Contains(authorLastName.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por país del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorCountryAsync(string authorCountry)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(
                b => b.Author.Country.ToLower().Contains(authorCountry.ToLower()),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por nombre y apellido del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorFullNameAsync(string authorName, string authorLastName)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync((
                b => b.Author.Name.ToLower().Contains(authorName.ToLower()) &&
                b.Author.LastName.ToLower().Contains(authorLastName.ToLower())),
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }

    // Traer libros por rango de fecha de nacimiento del autor
    public async Task<BaseMessage<Book>> GetBookByAuthorBirthDateRange(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var result = await _unitOfWork.BookRepository.GetAllAsync(
                b => b.Author.BirthDate >= startDate && b.Author.BirthDate <= endDate,
                includeProperties: "Author");
            return result.Any() ? Utilities.BuildResponse<Book>
                (HttpStatusCode.OK, BaseMessageStatus.OK_200, result) :
                Utilities.BuildResponse(HttpStatusCode.NotFound, BaseMessageStatus.BOOK_NOT_FOUND, new List<Book>());
        } catch (Exception ex)
        {
            return Utilities.BuildResponse<Book>(HttpStatusCode.InternalServerError, $"{BaseMessageStatus.INTERNAL_SERVER_ERROR_500} | {ex.Message}");
        }
    }
    #endregion
}