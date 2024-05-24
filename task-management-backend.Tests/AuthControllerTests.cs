using Microsoft.AspNetCore.Mvc;
using task_management_backend.Controllers;
using task_management_backend.Data;
using task_management_backend.Models.DTOs;

namespace task_management_backend.Tests;

public class AuthControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task Register_ReturnsOkResult_WithValidUser()
    {
        var context = new ApplicationDbContext(CreateDbContextOptions());
        var controller = new AuthController(context);
        var newUser = new UserRegisterDto
        {
            Username = "testuser",
            Password = "password123"
        };

        var result = await controller.Register(newUser);
        var okResult = Assert.IsType<OkResult>(result);
        Assert.NotNull(context.Users.SingleOrDefault(u => u.Username == "testuser"));
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WithInvalidUser()
    {
        var context = new ApplicationDbContext(CreateDbContextOptions());
        var controller = new AuthController(context);
        var newUser = new UserRegisterDto
        {
            Username = "",
            Password = ""
        };

        var result = await controller.Register(newUser);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}