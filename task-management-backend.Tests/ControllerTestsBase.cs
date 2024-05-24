using Microsoft.EntityFrameworkCore;
using task_management_backend.Data;

namespace task_management_backend.Tests;

public abstract class ControllerTestsBase
{
    protected static DbContextOptions<ApplicationDbContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
}