using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task_management_backend.Controllers;
using task_management_backend.Data;
using task_management_backend.Models.DTOs;
using TaskModel = task_management_backend.Models.Task;

namespace task_management_backend.Tests;

public class TasksControllerTests : ControllerTestsBase
{
    private static ClaimsPrincipal CreateMockUser(int userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, "testuser")
        };
        var identity = new ClaimsIdentity(claims, "TestDatabase");
        return new ClaimsPrincipal(identity);
    }

    private TasksController CreateMockController()
    {
        return new TasksController(new ApplicationDbContext(CreateDbContextOptions()))
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = CreateMockUser(1)
                }
            }
        };
    }

    [Fact]
    public async Task GetTasks_ReturnsEmptyList_WhenNoTasks()
    {
        var controller = CreateMockController();
        var result = await controller.GetTasks();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Task>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var tasks = Assert.IsType<List<TaskModel>>(okResult.Value);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task CreateTask_ReturnsCreatedAtActionResult_WithValidTask()
    {
        var controller = CreateMockController();
        var newTask = new TaskCreateDto
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = "New"
        };

        var result = await controller.CreateTask(newTask);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.NotNull(createdAtActionResult);
        var task = Assert.IsType<TaskModel>(createdAtActionResult.Value);
        Assert.Equal(newTask.Title, task.Title);
    }
}