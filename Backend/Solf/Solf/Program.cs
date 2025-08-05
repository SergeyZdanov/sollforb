using API.Mappers;
using Database;
using Database.Interfaces;
using Database.Repositoryes;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

/*var configuration = builder.Configuration;*/
builder.Services.AddControllers();

/*builder.Services.AddControllers().AddJsonOptions(options
    =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();


builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ClientMapper)));

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    /*options.UseNpgsql("Host=localhost;Port=5432;Database=Solf;Username=postgres;Password=111;");*/
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecretpassword;");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.MapControllers();

app.Run();
