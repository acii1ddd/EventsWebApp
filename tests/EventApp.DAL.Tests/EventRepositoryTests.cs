using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Repositories;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EventApp.DAL.Tests;

public class EventRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;

    // testing
    private readonly EventRepository _repository;
    
    public EventRepositoryTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EventProfile>();
        });
        
        _mapper = new Mapper(mapperConfig);
        _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        _mapperMock = new Mock<IMapper>();
        
        _repository = new EventRepository(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllEvents_ReturnsAllEvents()
    {
        // Arrange
        var eventEntities = new List<EventEntity>
        {
            new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event1",
                Description = "Test Description1",
                StartDate = DateTime.Now,
                Location = "Test Location1",
                Category = "Test Category1",
                MaxParticipants = 50,
            },
            
            new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event2",
                Description = "Test Description2",
                StartDate = DateTime.Now.AddDays(1),
                Location = "Test Location2",
                Category = "Test Category2",
                MaxParticipants = 80,
            }
        };
        
        var dbSetMock = new Mock<DbSet<EventEntity>>();
        
        dbSetMock.As<IQueryable<EventEntity>>()
            .Setup(m => m.Provider).Returns(eventEntities.AsQueryable().Provider);
        dbSetMock.As<IQueryable<EventEntity>>()
            .Setup(m => m.Expression).Returns(eventEntities.AsQueryable().Expression);
        dbSetMock.As<IQueryable<EventEntity>>()
            .Setup(m => m.ElementType).Returns(eventEntities.AsQueryable().ElementType);
        dbSetMock.As<IQueryable<EventEntity>>()
            .Setup(m => m.GetEnumerator())
            .Returns(eventEntities.GetEnumerator);

        _mapperMock.Setup(m => m.Map<List<EventModel>>(eventEntities))
            .Returns(_mapper.Map<List<EventModel>>(eventEntities));
        
        _contextMock.Setup(m => m.Events).Returns(dbSetMock.Object);
        
        // Act
        var result = await _repository.GetAllAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<EventEntity>>(result);
        Assert.Equal(result.Items.Count, eventEntities.Count);
        Assert.Equal(result.Items.First().Id, eventEntities.First().Id);
    }
}

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventModel, EventEntity>().ReverseMap();
    }
}
