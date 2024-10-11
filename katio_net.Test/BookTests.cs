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
public class BookTests
{
    // Variables
    private readonly IRepository<int, Book> _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookService _bookService;
    private List<Book> _books;

    // Constructor
    public BookTests()
    {
        _bookRepository = Substitute.For<IRepository<int, Book>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.BookRepository.Returns(_bookRepository);
        _bookService = new BookService(_unitOfWork);

        _books = new List<Book>()
        {
            new Book 
            { 
                Name = "Cien a�os de soledad",
                ISBN10 = "8420471836",
                ISBN13 = "978-8420471839",
                Published = new DateOnly(1967, 06, 05),
                Edition = "RAE Obra Acad�mica",
                DeweyIndex = "800",
                AuthorId = 1 
            },
            new Book 
            {
                Name = "Huellas",
                ISBN10 = "9584277278",
                ISBN13 = "978-958427275",
                Published = new DateOnly(2019, 01, 01),
                Edition = "1ra Edicion",
                DeweyIndex = "800",
                AuthorId = 3
            }
        };
    }


    // Test for getting all books
    [TestMethod]
    public async Task GetAllBooks()
    {
        // Arrange
        _bookRepository.GetAllAsync().Returns(_books);

        // Act
        var result = await _bookService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.ResponseElements.Count());
    }


    // Test for getting book by id
    [TestMethod]
    public async Task GetBookById()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.FindAsync(book.Id).Returns(book);

        // Act
        var result = await _bookService.GetBookById(book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(book.Name, result.ResponseElements.First().Name);
    }


    // Test for getting book by name
    [TestMethod]
    public async Task GetBookByName()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>()).Returns(new List<Book> { book });

        // Act
        var result = await _bookService.GetBooksByName(book.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(book.Name, result.ResponseElements.First().Name);
    }



    //Repository Exeptions
    // Test fot getting all books with repository exceptions
    [TestMethod]
    public async Task GetAllBooksRepositoryException()
    {
        // Arrange
        _bookRepository.GetAllAsync().Returns<Task<List<Book>>>(x => { throw new Exception("Repository error"); });

        // Act
        var result = await _bookService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
    }


    // Test for getting book by id with repository exceptions
    [TestMethod]
    public async Task GetBookByIdeRepositoryException()
    {
        // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.FindAsync(book.Id)).Do(x => throw new Exception());

        // Act
        var result = await _bookService.GetBookById(book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }


    // Test for getting book by name with repository exceptions
    [TestMethod]
    public async Task GetBookByAuthorIdRepositoryException()
    {
       // Arrange
        var book = _books.First();
        _bookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Book, bool>>>())).Do(x => throw new Exception());

        // Act
        var result = await _bookService.GetBookByAuthorNameAsync(book.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}