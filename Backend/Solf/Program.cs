using API.Mappers;
using AutoMapper;
using Database;
using Database.Interfaces;
using Database.Repositoryes;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

/*var configuration = builder.Configuration;*/
//builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IResourceService, ResourceService>();

builder.Services.AddScoped<IUeRepository, UeRepository>();
builder.Services.AddScoped<IUeService, UeService>();

builder.Services.AddScoped<IDocumentReceiptRepository, DocumentReceiptRepository>();
builder.Services.AddScoped<IDocumentReceiptService, DocumentReceiptService>(provider =>
    new DocumentReceiptService(
        provider.GetRequiredService<IDocumentReceiptRepository>(),
        provider.GetRequiredService<IBalanceService>(),
        provider.GetRequiredService<IResourceService>(),
        provider.GetRequiredService<IUeService>(),
        provider.GetRequiredService<IMapper>()
    )
);

builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();

builder.Services.AddScoped<IDocumentShippingRepository, DocumentShippingRepository>();
builder.Services.AddScoped<IDocumentShippingService, DocumentShippingService>();

builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ClientMapper)));
builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ResourceMapper)));
builder.Services.AddAutoMapper(x => x.AddProfile(typeof(UeMapper)));
builder.Services.AddAutoMapper(x => x.AddProfile(typeof(DocumentReceiptMapper)));
builder.Services.AddAutoMapper(x => x.AddProfile(typeof(DocumentShippingMapper)));

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    /* options.UseNpgsql("Host=localhost;Port=5432;Database=Solf;Username=postgres;Password=111;");*/
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=mysecretpassword;");
});

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}*/

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.Run();
