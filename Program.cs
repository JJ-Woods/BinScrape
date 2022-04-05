using BinScrape;
using Microsoft.Extensions.Options;

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .Configure<BinDaysConfig>(config.GetSection("BinDaysConfig"))
    .AddSingleton<IValidateOptions<BinDaysConfig>, BinDaysConfigValidator>()
    .AddSingleton<BinDaysConfig>(container => 
    {
        return container.GetService<IOptions<BinDaysConfig>>().Value;
    });           

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
