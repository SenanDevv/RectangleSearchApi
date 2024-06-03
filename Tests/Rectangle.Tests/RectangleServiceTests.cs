using Moq;
using Project.Core.Models;
using Project.Infrastructure.Repositories.Abstraction;
using Project.Service.Services.Implementation;
using System.Net;

namespace RectangleServiceTests;

public class RectangleServiceTests
{
    private readonly RectangleService _sut;
    private readonly Mock<IRectangleRepository> _rectangleRepository;

    public RectangleServiceTests()
    {
        var rectangleRepository = new Mock<IRectangleRepository>();
        _rectangleRepository = rectangleRepository;

        _sut = new RectangleService(rectangleRepository.Object);
    }

    [Fact]
    public async Task GetRectangle_ShouldReturnRectangle()
    {
        const int id = 1;

        // Arrange
        var rectangle = new RectangleModel()
        {
            Id = id,
            BottomRightPoint = new PointModel(1, 3),
            TopRightPoint = new PointModel(5, 2),
            TopLeftPoint = new PointModel(7, 4),
            BottomLeftPoint = new PointModel(2, 3),
        };

        _rectangleRepository.Setup( r =>  r.GetAsync(f => f.Id == id)).ReturnsAsync(rectangle);

        // Act
        var result = await _sut.GetRectangleAsync(id);

        // Assert
        _rectangleRepository.Verify(r => r.GetAsync(f => f.Id == id), Times.Once);

        Assert.True(result.Success);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(rectangle, result.Data);
    }
}
