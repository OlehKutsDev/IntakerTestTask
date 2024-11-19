using Azure.Messaging.ServiceBus;
using DummyListener;
using FluentValidation;
using FluentValidation.AspNetCore;
using IntakerTestTask.API.Validators.Task;
using IntakerTestTask.Application.Abstractions.Handlers;
using IntakerTestTask.Application.Abstractions.Repositories;
using IntakerTestTask.Application.Services.Task;
using IntakerTestTask.Infrastructure.Implementations.Handlers;
using IntakerTestTask.Persistence;
using IntakerTestTask.Persistence.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddSingleton<ServiceBusClient>(_ =>
{
    var connectionString = builder.Configuration.GetValue<string>("ServiceBus:ConnectionString");
    return new ServiceBusClient(connectionString);
});
builder.Services.AddSingleton<IServiceBusHandler, ServiceBusHandler>(provider =>
{
    var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
    var serviceBusSection = builder.Configuration.GetSection("ServiceBus");
    var topicName = serviceBusSection["TopicName"];
    var subscriptionName = serviceBusSection["SubscriptionName"];
    return new ServiceBusHandler(serviceBusClient, topicName!, subscriptionName!); 
});
builder.Services.AddHostedService<ServiceBusListener>();

// DB context
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=IntakerTestTaskDb;Username=postgres;Password=password1234$", 
        o => o.MigrationsAssembly("IntakerTestTask.Persistence")));

// Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskRequestDtoValidator>(ServiceLifetime.Singleton);
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTaskStatusRequestDtoValidator>(ServiceLifetime.Singleton);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWeb");

app.UseAuthorization();

app.MapControllers();

app.Run();