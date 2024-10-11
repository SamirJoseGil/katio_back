using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using katio.Data.Dto;
using System.Net;

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
                LastName = "Garc�a M�rquez",
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



    // Test for getting all authors
    [TestMethod]
    public async Task GetAllAuthors() 
    {
        // Arrange
        _authorRepository.GetAllAsync().Returns(_authors);

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.ResponseElements.Count());
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
        Assert.IsNotNull(result);
        Assert.AreEqual(author.Name, result.ResponseElements.First().Name);
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
        Assert.IsNotNull(result);
        Assert.AreEqual(author.Name, result.ResponseElements.First().Name);
    }


    // Repository Exeptions
    // Test for getting all authors with repository exceptions
    [TestMethod]
    public async Task GetAllAuthorsRepositoryException()
    {
        // Arrange
        _authorRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _authorService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
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
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
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
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}
