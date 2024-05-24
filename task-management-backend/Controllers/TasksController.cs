using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_backend.Data;
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
        var tasks = await dbContext.Tasks.ToListAsync();
        return Ok(tasks);
    }

    public async Task<ActionResult<Task>> GetTask(int id)
    {
        var task = await dbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

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
            DueDate = taskCreateDto.DueDate,
        };
        
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new {id = task.Id}, task);
    }

    public async Task<ActionResult<Task>> UpdateTask(int id, TaskUpdateDto taskUpdateDto)
    {
        var task = await dbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        
        task.Title = taskUpdateDto.Title;
        task.Description = taskUpdateDto.Description;
        task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        task.DueDate = taskUpdateDto.DueDate;
        task.Status = taskUpdateDto.Status;

        dbContext.Entry(task).State = EntityState.Modified;
        
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<Task>> DeleteTask(int id)
    {
        var task = await dbContext.Tasks.FindAsync(new
        {
            id = id,
            userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
        });
        if (task == null)
        {
            return NotFound();
        }

        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}