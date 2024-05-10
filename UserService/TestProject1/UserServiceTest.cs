using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService;
using UserService.Model;

namespace TestProject1;

[TestFixture]
public class UserServiceTest
{
    private UsersService _userService;
    private DataContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new DataContext(options);
        _userService = new UsersService(_context);

    }

    [Test]
    public async Task CreateUserAsync_Should_Add_New_User()
    {
        var user = new User { UserId = 1, Username = "testUser", Email = "test", PasswordHash = Encoding.UTF8.GetBytes("adadada"), PasswordSalt = Encoding.UTF8.GetBytes("salt")};

        var result = await _userService.CreateUserAsync(user);

        Assert.AreEqual(1, result.UserId);
        Assert.AreEqual("testUser", result.Username);
        Assert.AreEqual(1, await _context.Users.CountAsync());
    }

    [TearDown]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}