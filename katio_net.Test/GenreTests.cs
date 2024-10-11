using katio.Business.Interfaces;
using katio.Business.Services;
using katio.Data.Dto;
using katio.Data.Models;
using katio.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq.Expressions;
using System.Net;

namespace katio.Test;


[TestClass]
public class GenreTests
{
    private readonly IRepository<int, Genre> _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreService _genreService;
    private List<Genre> _genres;

    public GenreTests()
    {
        _genreRepository = Substitute.For<IRepository<int, Genre>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.GenreRepository.Returns(_genreRepository);
        _genreService = new GenreService(_unitOfWork);

        _genres = new List<Genre>()
        {
            new Genre
            {
                Name = "Fantasy",
                Description = "La Fantasia es..."
            },
            new Genre
            {
                Name = "Science Fiction",
                Description = "La Ciencia Ficcion es..."
            }
        };
    }

    // Test for getting all genres
    [TestMethod]
    public async Task GetAllGenres()
    {
        // Arrange
        _genreRepository.GetAllAsync().Returns(_genres);

        // Act
        var result = await _genreService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.ResponseElements.Count());
    }

    // Test for getting genre by id
    [TestMethod]
    public async Task GetGenreById()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.FindAsync(genre.Id).Returns(genre);

        // Act
        var result = await _genreService.GetByGenreId(genre.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(genre.Name, result.ResponseElements.First().Name);
    }

    // Test for getting genre by name
    [TestMethod]
    public async Task GetGenreByName()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>()).Returns(new List<Genre> { genre });

        // Act
        var result = await _genreService.GetGenresByName(genre.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(genre.Name, result.ResponseElements.First().Name);
    }



    // Repository Exeptions
    // Test for getting all genres
    [TestMethod]
    public async Task GetAllGenresRepositoryException()
    {
        // Arrange
        _genreRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }

    // Test for getting genre by id with repository exceptions
    [TestMethod]
    public async Task GetGenreByIdRepositoryException()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.When(x => x.FindAsync(genre.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.GetByGenreId(genre.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }

    // Test for getting genre by name with repository exceptions
    [TestMethod]
    public async Task GetGenreByNameRepositoryException()
    {
        // Arrange
        var genre = _genres.First();
        _genreRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Genre, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _genreService.GetGenresByName(genre.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}
