using System.ComponentModel;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Models;
using Moq;

namespace EventApp.DAL.Tests;

public class EventRepositoryTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    
    public EventRepositoryTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
    }

    [Fact]
    [DisplayName("Получение событий: должен вернуть вернуть заполненный cписок")]
    public async Task GetAllEvents_ReturnsAllEvents()
    {
        // Arrange
        var events = new PaginatedList<EventEntity>(
            new List<EventEntity>
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
            },
            pageIndex: 1,
            totalPages: 2
        );
        
        _eventRepositoryMock.Setup(repo => repo
                .GetAllAsync(1, 2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);
        
        // Act
        var result = await _eventRepositoryMock.Object.GetAllAsync(1, 2, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo.GetAllAsync(1, 2, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(events.Items.Count, result.Items.Count);
        Assert.Equal(result.Items[0].Description, events.Items[0].Description);
        Assert.Equal(result.Items[0].Category, events.Items[0].Category);
        Assert.Equal(result.Items[0].Location, events.Items[0].Location);
        Assert.Equal(result.Items[0].MaxParticipants, events.Items[0].MaxParticipants);
     
        Assert.Equal(result.Items[1].Description, events.Items[1].Description);
        Assert.Equal(result.Items[1].Category, events.Items[1].Category);
        Assert.Equal(result.Items[1].Location, events.Items[1].Location);
        Assert.Equal(result.Items[1].MaxParticipants, events.Items[1].MaxParticipants);
    }
    
    [Fact]
    [DisplayName("Получение событий, должен вернуть пустой список")]
    public async Task GetAllEvents_ReturnsEmptyList()
    {
        // Arrange
        var events = new PaginatedList<EventEntity>(new List<EventEntity> {}, 1, 2);
        
        _eventRepositoryMock.Setup(repo => repo
                .GetAllAsync(1, 2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);
        
        // Act
        var result = await _eventRepositoryMock.Object.GetAllAsync(1, 2, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo.GetAllAsync(1, 2, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Empty(result.Items);
    }
    
    [Fact]
    [DisplayName("Получение события по Id, должен вернуть событие")]
    public async Task GetById_ReturnsEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var eventEntity = new EventEntity
        {
            Id = id,
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        
        _eventRepositoryMock.Setup(repo => repo
            .GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object.GetByIdAsync(id, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(result.Description, eventEntity.Description);
        Assert.Equal(result.Category, eventEntity.Category);
        Assert.Equal(result.Location, eventEntity.Location);
        Assert.Equal(result.MaxParticipants, eventEntity.MaxParticipants);
    }
    
    [Fact]
    [DisplayName("Получение события по Id, должен вернуть null")]
    public async Task GetById_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _eventRepositoryMock.Setup(repo => repo
                .GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as EventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object.GetByIdAsync(id, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    [DisplayName("Получение события по Name, должен вернуть событие")]
    public async Task GetByName_ReturnsEvent()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        _eventRepositoryMock.Setup(repo => repo
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(result.Description, eventEntity.Description);
        Assert.Equal(result.Category, eventEntity.Category);
        Assert.Equal(result.Location, eventEntity.Location);
        Assert.Equal(result.MaxParticipants, eventEntity.MaxParticipants);
    }
    
    [Fact]
    [DisplayName("Получение события по Name, должен вернуть null")]
    public async Task GetByName_ReturnsNull()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        _eventRepositoryMock.Setup(repo => repo
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as EventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .GetByNameAsync(eventEntity.Name, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.Null(result);
    }
    
    [Fact]
    [DisplayName("Добавление события, должен вернуть добавленное событие")]
    public async Task AddEvent_ReturnsAddedEvent()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        _eventRepositoryMock.Setup(repo => repo
            .AddAsync(eventEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .AddAsync(eventEntity, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .AddAsync(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(result.Description, eventEntity.Description);
        Assert.Equal(result.Category, eventEntity.Category);
        Assert.Equal(result.Location, eventEntity.Location);
        Assert.Equal(result.MaxParticipants, eventEntity.MaxParticipants);
    }
    
    [Fact]
    [DisplayName("Обновление события, должен вернуть обновленное событие")]
    public async Task UpdateEvent_ReturnsUpdatedEvent()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        _eventRepositoryMock.Setup(repo => repo
            .UpdateAsync(eventEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .UpdateAsync(eventEntity, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .UpdateAsync(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(result.Description, eventEntity.Description);
        Assert.Equal(result.Category, eventEntity.Category);
        Assert.Equal(result.Location, eventEntity.Location);
        Assert.Equal(result.MaxParticipants, eventEntity.MaxParticipants);
    }

    [Fact]
    [DisplayName("Удаление события, должен вернуть удаленное событие")]
    public async Task DeleteEvent_ReturnsDeletedEvent()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.Now,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
        };
        _eventRepositoryMock.Setup(repo => repo
            .DeleteAsync(eventEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .DeleteAsync(eventEntity, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .DeleteAsync(eventEntity, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(result.Description, eventEntity.Description);
        Assert.Equal(result.Category, eventEntity.Category);
        Assert.Equal(result.Location, eventEntity.Location);
        Assert.Equal(result.MaxParticipants, eventEntity.MaxParticipants);
    }
    
    [Fact]
    [DisplayName("Получение событий по критериям, должен вернуть список событий")]
    public async Task GetByFilter_ReturnsEvents()
    {
        // Arrange
        var events = new PaginatedList<EventEntity>(
            new List<EventEntity>
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
            },
            pageIndex: 1,
            totalPages: 2
        );
        
        _eventRepositoryMock.Setup(repo => repo
                .GetByFilterAsync(DateTime.Parse("01-01-2024"), "location", "category",1, 2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);
        
        // Act
        var result = await _eventRepositoryMock.Object
            .GetByFilterAsync(DateTime.Parse("01-01-2024"), "location", "category",1, 2, It.IsAny<CancellationToken>());
        
        // Asserts
        _eventRepositoryMock.Verify(repo => repo
            .GetByFilterAsync(DateTime.Parse("01-01-2024"), "location", "category",1, 2, It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.Equal(events.Items.Count, result.Items.Count);
        Assert.Equal(result.Items[0].Description, events.Items[0].Description);
        Assert.Equal(result.Items[0].MaxParticipants, events.Items[0].MaxParticipants);
        Assert.Equal(result.Items[1].Description, events.Items[1].Description);
        Assert.Equal(result.Items[1].MaxParticipants, events.Items[1].MaxParticipants);
    }
}