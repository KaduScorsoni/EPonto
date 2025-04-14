using Application.Interfaces;
using Application.Services;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura��o dos servi�os
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Adiciona o Swagger ao projeto
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Adiciona os servi�os necess�rios da aplica��o
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<DbSession>();

var app = builder.Build();

// Configura o middleware do Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Exibe o Swagger na raiz (opcional)
});

// Configura��o do pipeline de requisi��es
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
