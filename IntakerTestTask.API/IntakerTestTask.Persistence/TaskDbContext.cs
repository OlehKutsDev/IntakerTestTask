using IntakerTestTask.Application.Common.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace IntakerTestTask.Persistence;

public class TaskDbContext : DbContext
{
    public DbSet<TaskModel> Tasks { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tasks
        modelBuilder.Entity<TaskModel>().ToTable("Tasks");
        modelBuilder.Entity<TaskModel>().HasKey(t => t.Id);
        modelBuilder.Entity<TaskModel>().Property(t => t.Name).IsRequired();
        modelBuilder.Entity<TaskModel>().Property(t => t.Description).IsRequired();
        modelBuilder.Entity<TaskModel>().Property(t => t.Status).IsRequired();
    }
}