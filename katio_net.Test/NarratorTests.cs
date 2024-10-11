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
        Assert.AreEqual(narrator.Name, result.ResponseElements.First().Name);
    }


    // Repository Exeptions
    // Test for getting a narrator by Id with repository exceptions
    [TestMethod]
    public async Task GetNarratorByIdRepositoryException()
    {
        // Arrange
        var narratorId = 1;
        _narratorRepository.FindAsync(narratorId).Returns<Task<Narrator>>( x => { throw new Exception("Repository error"); });

        // Act
        var result = await _narratorService.GetNarratorById(narratorId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
    }


    // Test for getting a narrator by name with repository exceptions
    [TestMethod]
    public async Task GetNarratorsByNameRepositoryException()
    {
        // Arrange
        var narrator = _narrators.First();
        _narratorRepository.GetAllAsync(Arg.Any<Expression<Func<Narrator, bool>>>()).Returns<Task<List<Narrator>>>(x => { throw new Exception("Repository error"); });

        // Act
        var result = await _narratorService.GetNarratorsByName(narrator.Name);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.AreEqual(BaseMessageStatus.INTERNAL_SERVER_ERROR_500, result.Message);
    }



}
