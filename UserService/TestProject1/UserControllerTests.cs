using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Model;
namespace TestProject1;

[TestFixture]
public class UserControllerTests
{
    private Mock<IUsersService> _userServiceMock;
    private UsersController _controller;

    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUsersService>();
        _controller = new UsersController(_userServiceMock.Object);
    }

    [Test]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var user = new User { UserId = 1, Username = "testUser", Email = "test@example.com" };
        _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

        var actionResult = await _controller.GetUserById(1);

        Assert.IsTrue(actionResult.Result is OkObjectResult);
        var okResult = actionResult.Result as OkObjectResult;
        Assert.AreEqual(200, okResult?.StatusCode);
        Assert.AreEqual(user, okResult?.Value);
    }
    
    [Test]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

        var actionResult = await _controller.GetUserById(1);

        Assert.IsTrue(actionResult.Result is NotFoundObjectResult);
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}