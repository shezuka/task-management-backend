using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_backend.Data;
using task_management_backend.Helpers;
using task_management_backend.Models.DTOs;

namespace task_management_backend.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class TasksController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
    {
        var tasks = await dbContext.Tasks
            .OrderBy(t => t.Id)
            .ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Task>> GetTask(int id)
    {
        var task = await dbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<Task>> CreateTask(TaskCreateDto taskCreateDto)
    {
        var sUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var userId = int.Parse(sUserId);

        var task = new Models.Task
        {
            Title = taskCreateDto.Title,
            Description = taskCreateDto.Description,
            UserId = userId,
            Status = taskCreateDto.Status,
            DueDate = taskCreateDto.DueDate.SetKindUtc(),
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Task>> UpdateTask(int id, TaskUpdateDto taskUpdateDto)
    {
        var task = await dbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Title = taskUpdateDto.Title;
        task.Description = taskUpdateDto.Description;
        task.DueDate = taskUpdateDto.DueDate.SetKindUtc();
        task.Status = taskUpdateDto.Status;

        dbContext.Entry(task).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Task>> DeleteTask(int id)
    {
        await dbContext.Tasks
            .Where(x => x.Id == id && x.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))
            .ExecuteDeleteAsync();
        return NoContent();
    }
}