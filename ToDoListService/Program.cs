using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using ToDoListService.Configurations;
using ToDoListService.Consumers;
using ToDoListService.Data;
using ToDoListService.Data.Interfaces;
using ToDoListService.Jobs;
using ToDoListService.Jobs.Interfaces;
using ToDoListService.Middlewares;
using ToDoListService.Repositories;
using ToDoListService.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext")));
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<ITodoItemsGenerationJob, TodoItemGenerationJob>();

// Jobs
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"))
);
builder.Services.AddHangfireServer();

// MassTransit
builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<TodoItemsConsumer>();
        configuration.UsingRabbitMq((context, cfg) =>
        {
            var rabbitmqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitmqSettings>();
            cfg.Host(rabbitmqSettings.Host, "/");
            cfg.ConfigureEndpoints(context);
        });
    }
);

// NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();

app.UseErrorResponseDecoration();
app.UseElapsedResponseTimeHeader();

RecurringJob.AddOrUpdate<ITodoItemsGenerationJob>("TODOS_GENERATOR",
    job => job.RunAsync(app.Lifetime.ApplicationStopping), Cron.Minutely);

app.Run();

public partial class Program;