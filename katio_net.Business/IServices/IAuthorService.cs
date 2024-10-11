using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface IAuthorService
{
    Task<BaseMessage<Author>> Index();
    Task<BaseMessage<Author>> GetAuthorById(int Id);
    Task<BaseMessage<Author>> GetAuthorsByName(string Name);
    Task<BaseMessage<Author>> GetAuthorsByLastName(string LastName);
    Task<BaseMessage<Author>> GetAuthorsByBirthDate(DateOnly StartDate, DateOnly EndDate);
    Task<BaseMessage<Author>> GetAuthorsByCountry(string Country);
    Task<BaseMessage<Author>> DeleteAuthor(int Id);
    Task<BaseMessage<Author>> CreateAuthor(Author author);
    Task<BaseMessage<Author>> UpdateAuthor(Author author);
}

