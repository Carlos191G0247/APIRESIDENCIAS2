
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/// JWT
/// 
string audence = "localhost:7136";
var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("residencias9.1G1234567890"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
{

    jwt.Audience = audence;

    jwt.TokenValidationParameters = new TokenValidationParameters
    {

        IssuerSigningKey = llave,
        ValidIssuer = "https://localhost:7136",
        ValidAudience = audence,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
    };

});
//

builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Sistem21ResidenciaswebcaContext>(optionsBuilder => 
optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_residenciaswebca;user=sistem21_residenciasca;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.20-mariadb")));

builder.Services.AddTransient<SolicitudesRepository>();
builder.Services.AddTransient<ResidentesRepository>();
builder.Services.AddTransient<ArchivosenviadorRepository>();
builder.Services.AddTransient<IniciarSesionRepository>();
builder.Services.AddTransient<AsignartareasRepository>();
builder.Services.AddTransient<CoordinadoresRepository>();
builder.Services.AddTransient<CarreraRepository>();
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

// Agregar UseStaticFiles y UseDefaultFiles aquí
app.UseStaticFiles();
app.UseDefaultFiles();

app.UseRouting();

app.UseAuthentication();  // Asegúrate de agregar esta línea antes de UseAuthorization

app.UseAuthorization();

app.MapControllers();
app.Run();
