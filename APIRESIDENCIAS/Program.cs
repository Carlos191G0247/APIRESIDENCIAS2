
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Sistem21ResidenciaswebcaContext>(optionsBuilder => 
optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_residenciaswebca;user=sistem21_residenciasca;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.20-mariadb")));

builder.Services.AddTransient<SolicitudesRepository>();
builder.Services.AddTransient<ResidentesRepository>();
builder.Services.AddTransient<ArchivosenviadorRepository>();
builder.Services.AddTransient<IniciarSesionRepository>();
builder.Services.AddTransient<Residentes>();
builder.Services.AddSignalR();

var app = builder.Build();


app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.MapControllers();
app.Run();
