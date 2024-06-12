using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using BCILLogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));});
builder.Services.AddSingleton<Logger>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File("logs/Glen.txt", rollingInterval: RollingInterval.Day) // Add file logging, adjust the file path as needed
    .CreateLogger();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog(); // Use Serilog for logging
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","Glen API V1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
