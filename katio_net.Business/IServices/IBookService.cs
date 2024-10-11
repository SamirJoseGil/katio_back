using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IBookService
{
    Task<BaseMessage<Book>> Index();
    Task<BaseMessage<Book>> GetBookById(int Id);
    Task<BaseMessage<Book>> GetBooksByName(string Name);
    Task<BaseMessage<Book>> GetBooksByISBN10(string ISBN10);
    Task<BaseMessage<Book>> GetBooksByISBN13(string ISBN13);
    Task<BaseMessage<Book>> GetBooksByEdition(string Edition);
    Task<BaseMessage<Book>> GetBooksByDeweyIndex(string DeweyIndex);
    Task<BaseMessage<Book>> GetBooksByPublished(DateOnly StartDate, DateOnly EndDate);
    Task<BaseMessage<Book>> DeleteBook(int id);
    Task<BaseMessage<Book>> CreateBook(Book book);
    Task<BaseMessage<Book>> UpdateBook(Book book);


    Task<BaseMessage<Book>> GetBookByAuthorAsync(int AuthorId);
    Task<BaseMessage<Book>> GetBookByAuthorNameAsync(string AuthorName);
    Task<BaseMessage<Book>> GetBookByAuthorCountryAsync(string AuthorCountry);
    Task<BaseMessage<Book>> GetBookByAuthorFullNameAsync(string authorName, string authorLastName);
    Task<BaseMessage<Book>> GetBookByAuthorBirthDateRange(DateOnly StartDate, DateOnly EndDate);
}
