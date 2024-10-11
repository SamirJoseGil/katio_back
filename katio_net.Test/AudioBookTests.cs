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
public class AudioBookTests
{
    private readonly IRepository<int, AudioBook> _audioBookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAudioBookService _audioBookService;
    private List<AudioBook> _audioBooks;

    public AudioBookTests()
    {
        _audioBookRepository = Substitute.For<IRepository<int, AudioBook>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.AudioBookRepository.Returns(_audioBookRepository);
        _audioBookService = new AudioBookService(_unitOfWork);

        _audioBooks = new List<AudioBook>()
        {
            new AudioBook
            {
                Id = 1,
                Name = "Cien a�os de soledad",
                ISBN10 = "8420471836",
                ISBN13 = "978-8420471839",
                Published = new DateOnly(1967, 06, 05),
                Edition = "RAE Obra Acad�mica",
                Genre = "Ficcion",
                LenghtInSeconds = 1,
                Path = "C:/Users/Usuario/Downloads/Cien a�os de soledad.mp3",
                AuthorId = 1
            },
            new AudioBook
            {
                Id = 2,
                Name = "Huellas",
                ISBN10 = "9584277278",
                ISBN13 = "978-958427275",
                Published = new DateOnly(2019, 01, 01),
                Edition = "1ra Edicion",
                Genre = "Ficcion",
                LenghtInSeconds = 1,
                Path = "C:/Users/Usuario/Downloads/Huellas.mp3",
                AuthorId = 3
            }
        };
    }


    // Test for getting all audio books
    [TestMethod]
    public async Task GetAllAudioBooks() 
    {
        // Arrange 
        _audioBookRepository.GetAllAsync().Returns(_audioBooks);

        // Act
        var result = await _audioBookService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.ResponseElements.Count());
    }


    // Test for getting an audiobook by id
    [TestMethod]
    public async Task GetAudioBookById()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.FindAsync(audioBook.Id).Returns(audioBook);

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(audioBook.Name, result.ResponseElements.First().Name);
    }


    // Test for getting audioBook by name 
    [TestMethod]
    public async Task GetAudioBookByName()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>()).Returns(new List<AudioBook> { audioBook });

        // Act
        var result = await _audioBookService.GetByAudioBookName(audioBook.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(audioBook.Name, result.ResponseElements.First().Name);
    }



    // Repository Exeptions
    // Test for getting all audio books with repository exceptions
    [TestMethod]
    public async Task GetAllAudioBooksRepositoryException()
    {
        // Arrange
        _audioBookRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }


    // Test for getting audioBook by id with repository exceptions
    [TestMethod]
    public async Task GetAudioBookByIdRepositoryException()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.When(x => x.FindAsync(audioBook.Id)).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.GetAudioBookById(audioBook.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }


    // Test for getting audioBook by name with repository exceptions
    [TestMethod]
    public async Task GetAudioBookByNameRepositoryException()
    {
        // Arrange
        var audioBook = _audioBooks.First();
        _audioBookRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<AudioBook, bool>>>())).Do(x => throw new Exception());

        // Act
        var result = await _audioBookService.GetByAudioBookName(audioBook.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}
