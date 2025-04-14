using Application.Interfaces;
using Application.Services;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Definir a porta (verificando o ambiente)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Porta padrão para Railway
builder.WebHost.UseUrls($"http://*:{port}");

// Adicionar serviços à aplicação
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração dos serviços
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<DbSession>();

var app = builder.Build();

// Habilitar o Swagger se estiver no ambiente de Desenvolvimento ou Produção
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty; // Exibe o Swagger diretamente na raiz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
