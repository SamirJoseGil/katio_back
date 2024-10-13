using NSubstitute;
using katio.Data;
using katio.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using katio.Business.Interfaces;
using katio.Business.Services;
using System.Linq.Expressions;
using katio.Data.Dto;
using System.Net;
using System.Xml.Linq;


namespace katio.Test;

public class NarratorTests
{
    private readonly IRepository<int, Narrator> _narratorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INarratorService _narratorService;
    private List<Narrator> _narrators;

    public NarratorTests()
    {
        _narratorRepository = Substitute.For<IRepository<int, Narrator>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.NarratorRepository.Returns(_narratorRepository);
        _narratorService = new NarratorService(_unitOfWork);

        _narrators = new List<Narrator>()
        {
            new Narrator
            {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
            },
            new Narrator
            {
            Name = "Juan",
            LastName = "Perez",
            Genre = "Ficcion"
            }
        };
    }


    #region Test Methods Simple

    // Test for getting all narrators
    [TestMethod]
    public async Task GetAllNarrators() 
    {
        // Arrange
        _narratorRepository.GetAllAsync().Returns(_narrators);

        // Act
        var result = await _narratorService.Index();

        // Assert
        Assert.AreEqual(2, result.ResponseElements.Count());
    }


    // Test for creating a narrator
    [TestMethod]
    public async Task CreateNarrator()
    {
        // Arrange
        var narrator = new Narrator
        {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator>());
        _narratorRepository.AddAsync(narrator).Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.CreateNarrator(narrator);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(narrator.Name, result.ResponseElements.First().Name);
        Assert.AreEqual(narrator.LastName, result.ResponseElements.First().LastName);
        Assert.AreEqual(narrator.Genre, result.ResponseElements.First().Genre);
    }


    // Test for updating a narrator
    [TestMethod]
    public async Task UpdateNarrator()
    {
        // Arrange
        var narratorToUpdate = _narrators.First();
        _narratorRepository.FindAsync(narratorToUpdate.Id).Returns(narratorToUpdate);

        var updatedNarrator = new Narrator
        {
            Id = narratorToUpdate.Id,
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };
        _narratorRepository.FindAsync(updatedNarrator.Id).Returns(updatedNarrator);

        _narratorRepository.Update(updatedNarrator).Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.UpdateNarrator(updatedNarrator);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(updatedNarrator.Name, result.ResponseElements.First().Name);
        Assert.AreEqual(updatedNarrator.LastName, result.ResponseElements.First().LastName);
        Assert.AreEqual(updatedNarrator.Genre, result.ResponseElements.First().Genre);
    }


    // Test for deleting a narrator
    [TestMethod]
    public async Task DeleteNarrator()
    {
        // Arrange
        var narratorToDelete = _narrators.First();
        _narratorRepository.FindAsync(narratorToDelete.Id).Returns(narratorToDelete);

        _narratorRepository.Delete(narratorToDelete).Returns(Task.CompletedTask);

        // Act
        var result = await _narratorService.DeleteNarrator(narratorToDelete.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        await _narratorRepository.Received().Delete(narratorToDelete);
    }


    // Test for getting a narrator by Id
    [TestMethod]
    public async Task GetNarratorById() 
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.FindAsync(narrator.Id).Returns(narrator);

        // Act
        var result = await _narratorService.GetNarratorById(narrator.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(narrator.Name, result.ResponseElements.First().Name);
    }


    // Test for getting a narrator by Name
    [TestMethod]
    public async Task GetNarratorsByName() 
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByName(narrator.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(narrator.Name, result.ResponseElements.First().Name);
    }


    // Test for getting a narrator by Last Name
    [TestMethod]
    public async Task GetNarratorsByLastName()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByLastName(narrator.LastName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(narrator.LastName, result.ResponseElements.First().LastName);
    }


    // Test for getting a narrator by Genre
    [TestMethod]
    public async Task GetNarratorsByGenre()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator> { narrator });

        // Act
        var result = await _narratorService.GetNarratorsByGenre(narrator.Genre);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.OK_200, result.Message);
        Assert.AreEqual(narrator.Genre, result.ResponseElements.First().Genre);
    }
    #endregion

    #region Repository Exceptions

    // Test for getting all narrators with repository exceptions
    [TestMethod]
    public async Task GetAllNarratorsRepositoryException()
    {
        // Arrange
        _narratorRepository.When(x => x.GetAllAsync()).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.Index();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for creating a narrator with repository exceptions
    [TestMethod]
    public async Task CreateNarratorRepositoryException()
    {
        // Arrange
        var narrator = new Narrator
        {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };

        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns(new List<Narrator>());

        _narratorRepository.When(x => x.AddAsync(Arg.Any<Narrator>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.CreateNarrator(narrator);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for updating a narrator with repository exceptions
    [TestMethod]
    public async Task UpdateNarratorRepositoryException()
    {
        // Arrange
        var narratorToUpdate = _narrators.First();
        _narratorRepository.FindAsync(narratorToUpdate.Id).Returns(narratorToUpdate);

        var updatedNarrator = new Narrator
        {
            Id = narratorToUpdate.Id,
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        };

        _narratorRepository.FindAsync(updatedNarrator.Id).Returns(updatedNarrator);

        _narratorRepository.When(x => x.Update(updatedNarrator)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.UpdateNarrator(updatedNarrator);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for deleting a narrator with repository exceptions
    [TestMethod]
    public async Task DeleteNarratorRepositoryException()
    {
        // Arrange
        var narratorToDelete = _narrators.First();
        _narratorRepository.FindAsync(narratorToDelete.Id).Returns(narratorToDelete);

        _narratorRepository.When(x => x.Delete(narratorToDelete)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.DeleteNarrator(narratorToDelete.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for getting a narrator by Id with repository exceptions
    [TestMethod]
    public async Task GetNarratorByIdRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.FindAsync(narrator.Id)).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorById(narrator.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for getting a narrator by name with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByNameRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByName(narrator.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for getting a narrator by last name with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByLastNameRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByLastName(narrator.LastName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }


    // Test for getting a narrator by genre with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByGenreRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.When(x => x.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>())).Do(x => throw new Exception("Repository error"));

        // Act
        var result = await _narratorService.GetNarratorsByGenre(narrator.Genre);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
        Assert.AreEqual("500 Internal Server Error | Repository error", result.Message);
    }

    #endregion
}
