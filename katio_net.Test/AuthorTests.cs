using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using katio.Data.Dto;
using System.Net;
using NSubstitute.ExceptionExtensions;

namespace katio.Test;


[TestClass]
public class AuthorTests
{
    private readonly IRepository<int, Author> _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorService _authorService;
    private List<Author> _authors;


    public AuthorTests()
    {
        _authorRepository = Substitute.For<IRepository<int, Author>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.AuthorRepository.Returns(_authorRepository);
        _authorService = new AuthorService(_unitOfWork);

        _authors = new List<Author>()
        {
            new Author 
            {
                Name = "Gabriel",
                LastName = "García Márquez",
                Country = "Colombia",
                BirthDate = new DateOnly(1940, 03, 03)
            },
            new Author 
            {
                Name = "Jorge",
                LastName = "Isaacs",
                Country = "Colombia",
                BirthDate = new DateOnly(1836, 04, 01)            
            }
        };
    }


    #region Test Methods Simple

    // Test for getting all authors
    [TestMethod]
    public async Task GetAllAuthors() 
    {
        // Arrange
        _authorRepository.GetAllAsync().Returns(_authors);

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
    }


    // Test for creating author
    [TestMethod]
    public async Task CreateAuthor()
    {
        // Arrange
        var newAuthor = new Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author>());
        _authorRepository.AddAsync(newAuthor).Returns(Task.CompletedTask);

        // Act
        var result = await _authorService.CreateAuthor(newAuthor);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for updating author
    [TestMethod]
    public async Task UpdateAuthor()
    {
        // Arrange
        var authorToUpdate = _authors.First();
        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);

        var updatedAuthor = new Author
        {
            Id = authorToUpdate.Id,
            Name = "Gabriel Updated",
            LastName = "García Márquez Updated",
            Country = "Colombia Updated",
            BirthDate = new DateOnly(1950, 01, 01)
        };
        _authorRepository.FindAsync(updatedAuthor.Id).Returns(updatedAuthor);

        _authorRepository.Update(updatedAuthor).Returns(Task.CompletedTask);        

        // Act
        var result = await _authorService.UpdateAuthor(updatedAuthor);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for deleting author
    [TestMethod]
    public async Task DeleteAuthor()
    {
        // Arrange
        var authorToDelete = _authors.First();
        _authorRepository.FindAsync(authorToDelete.Id).Returns(authorToDelete);

        _authorRepository.Delete(authorToDelete).Returns(Task.CompletedTask);

        // Act
        var result = await _authorService.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for getting author by id
    [TestMethod]
    public async Task GetAuthorById()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.FindAsync(author.Id).Returns(author);

        // Act
        var result = await _authorService.GetAuthorById(author.Id);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for getting author by name
    [TestMethod]
    public async Task GetAuthorByName()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByName(author.Name);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for getting author by last name
    [TestMethod]
    public async Task GetAuthorByLastName()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByLastName(author.LastName);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }


    // Test for getting author by birth date
    [TestMethod]
    public async Task GetAuthorsByBirthDate()
    {
        // Arrange
        var endDate = new DateOnly(1950, 12, 31);
        var startDate = new DateOnly(1830, 01, 01);
        var expectedAuthors = _authors.Where(a => a.BirthDate >= startDate && a.BirthDate <= endDate).ToList();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(expectedAuthors);

        // Act
        var result = await _authorService.GetAuthorsByBirthDate(startDate, endDate);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        Assert.AreEqual(2, result.ResponseElements.Count());
        }


    // Test for getting author by country
    [TestMethod]
    public async Task GetAuthorByCountry()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author> { author });

        // Act
        var result = await _authorService.GetAuthorsByCountry(author.Country);

        // Assert
        Assert.IsTrue(result.ResponseElements.Any());
        }
    #endregion

    #region Repository Exceptions

    // Test for getting all authors with repository exceptions
    [TestMethod]
    public async Task GetAllAuthorsRepositoryException()
    {
        // Arrange
        _authorRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for creating author with repository exceptions
    [TestMethod]
    public async Task CreateAuthorRepositoryException()
    {
        // Arrange
        var newAuthor = new Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };

        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).Returns(new List<Author>());

        _authorRepository.When(x => x.AddAsync(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.CreateAuthor(newAuthor);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for updating author with repository exceptions
    [TestMethod]
    public async Task UpdateAuthorRepositoryException()
    {
        // Arrange
        var authorToUpdate = _authors.First();
        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);

        var updatedAuthor = new Author
        {
            Id = authorToUpdate.Id,
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };

        _authorRepository.FindAsync(authorToUpdate.Id).Returns(authorToUpdate);

        _authorRepository.When(x => x.Update(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.UpdateAuthor(updatedAuthor);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for deleting author with repository exceptions
    [TestMethod]
    public async Task DeleteAuthorRepositoryException()
    {
        // Arrange
        var authorToDelete = _authors.First();
        _authorRepository.FindAsync(authorToDelete.Id).Returns(authorToDelete);

        _authorRepository.When(x => x.Delete(Arg.Any<Author>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for getting author by id with repository exceptions
    [TestMethod]
    public async Task GetAuthorByIdRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.FindAsync(author.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorById(author.Id);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for getting author by name with repository exceptions
    [TestMethod]
    public async Task GetAuthorByNameRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByName(author.Name);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for getting author by last name with repository exceptions
    [TestMethod]
    public async Task GetAuthorByLastNameRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByLastName(author.LastName);

        // Assert
        Assert.AreEqual((int)result.StatusCode, 500);
    }


    // Test for getting author by birth date with repository exceptions
    [TestMethod]
    public async Task GetAuthorsByBirthDateRepositoryException()
    {
        // Arrange
        var startDate = new DateOnly(1830, 01, 01);
        var endDate = new DateOnly(1950, 12, 31);
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByBirthDate(startDate, endDate);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }


    // Test for getting author by country with repository exceptions
    [TestMethod]
    public async Task GetAuthorByCountryRepositoryException()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.GetAuthorsByCountry(author.Country);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }

    #endregion

    #region Test Methods Fail

    // Test for getting all authors Fail
    [TestMethod]
    public async Task GetAllAuthorsFail()
    {
        // Arrange
        _authorRepository.GetAllAsync().ReturnsForAnyArgs(new List<Author>());

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for creating author Fail
    [TestMethod]
    public async Task CreateAuthorFail()
    {
        // Arrange
        var existingAuthor = new Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };

        var newAuthor = new Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        };

        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>())
            .ReturnsForAnyArgs(new List<Author> { existingAuthor });

        // Act
        var result = await _authorService.CreateAuthor(newAuthor);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
        Assert.IsTrue(result.Message.Contains(BaseMessageStatus.BAD_REQUEST_400));
    }
    // Test for updating author Fail
    [TestMethod]
    public async Task UpdateAuthorFail()
    {
        // Arrange
        _authorRepository.Update(Arg.Any<Author>()).ThrowsAsyncForAnyArgs(new Exception());
        _unitOfWork.AuthorRepository.Returns(_authorRepository);

        // Act
        var result = await _authorService.UpdateAuthor(new Author());

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }
    // Test for deleting author Fail
    [TestMethod]
    public async Task DeleteAuthorFail()
    {
        // Arrange
        var authorToDelete = _authors.First();
        _authorRepository.FindAsync(authorToDelete.Id).ReturnsForAnyArgs(Task.FromResult<Author?>(null));

        // Act
        var result = await _authorService.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting author by id Fail
    [TestMethod]
    public async Task GetAuthorByIdFail()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.FindAsync(author.Id).ReturnsForAnyArgs(Task.FromResult<Author?>(null));

        // Act
        var result = await _authorService.GetAuthorById(author.Id);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting author by name Fail
    [TestMethod]
    public async Task GetAuthorByNameFail()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).ReturnsForAnyArgs(new List<Author>());

        // Act
        var result = await _authorService.GetAuthorsByName(author.Name);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting author by last name Fail
    [TestMethod]
    public async Task GetAuthorByLastNameFail()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).ReturnsForAnyArgs(new List<Author>());

        // Act
        var result = await _authorService.GetAuthorsByLastName(author.LastName);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting author by birth date Fail
    [TestMethod]
    public async Task GetAuthorsByBirthDateFail()
    {
        // Arrange
        var startDate = new DateOnly(1830, 01, 01);
        var endDate = new DateOnly(1950, 12, 31);
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).ReturnsForAnyArgs(new List<Author>());

        // Act
        var result = await _authorService.GetAuthorsByBirthDate(startDate, endDate);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }
    // Test for getting author by country Fail
    [TestMethod]
    public async Task GetAuthorByCountryFail()
    {
        // Arrange
        var author = _authors.First();
        _authorRepository.GetAllAsync(Arg.Any<Expression<Func<Author, bool>>>()).ReturnsForAnyArgs(new List<Author>());

        // Act
        var result = await _authorService.GetAuthorsByCountry(author.Country);

        // Assert
        Assert.IsFalse(result.ResponseElements.Any());
    }

    #endregion
}
