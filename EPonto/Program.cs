using Application.Interfaces;
using Application.Jobs;
using Application.Services;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Hangfire;
using Hangfire.MySql;
using Hangfire.Storage;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Web.Http.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Domain.Entities.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:8080"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Se estiver usando cookies/autenticação
    });
});


//Propiedade para fazer mapemaneto de entidades com nome no banco diferente
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new string[] {}
        }
    });
});

//Login
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

//Feriado 
builder.Services.AddScoped<IFeriadoService, FeriadoService>();
builder.Services.AddScoped<IFeriadoRepository, FeriadoRepository>();

//Ferias
builder.Services.AddScoped<IFeriasService, FeriasService>();
builder.Services.AddScoped<IFeriasRepository, FeriasRepository>();

//Calendario 
builder.Services.AddScoped<ICalendarioService, CalendarioService>();

//Comunicado
builder.Services.AddScoped<IComunicadoService, ComunicadoService>();
builder.Services.AddScoped<IComunicadoRepository, ComunicadoRepository>();

//Usuario
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

//Ponto
builder.Services.AddScoped<IRegistroPontoService, RegistroPontoService>();
builder.Services.AddScoped<IRegistroPontoRepository, RegistroPontoRepository>();

//Cargo
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();

//Jornada de trabalho
builder.Services.AddScoped<IJornadaTrabalhoService, JornadaTrabalhoService>();
builder.Services.AddScoped<IJornadaTrabalhoRepository, JornadaTrabalhoRepository>();

//Banco Horas
builder.Services.AddScoped<IBancoHorasService, BancoHorasService>();
builder.Services.AddScoped<IBancoHorasRepository, BancoHorasRepository>();

//Feedback
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

//SolicitacaoAusencia
builder.Services.AddScoped<ISolicitacaoAusenciaService, SolicitacaoAusenciaService>();
builder.Services.AddScoped<ISolicitacaoAusenciaRepository, SolicitacaoAusenciaRepository>();

//PERFIL DE USUÁRIO
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();

builder.Services.AddScoped<DbSession>();

// Hangfire - Configuração com MySQL
builder.Services.AddHangfire(configuration =>
{
    configuration.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(new MySqlStorage(
                        builder.Configuration.GetConnectionString("HangfireConnection"),
                        new MySqlStorageOptions
                        {
                            TablesPrefix = "Hangfire"
                        }));
});

builder.Services.AddHangfireServer();

// Adicione o serviço do job
builder.Services.AddScoped<BancoHorasJob>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddSingleton(sp =>
{
    var cfg = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(cfg.CloudName, cfg.ApiKey, cfg.ApiSecret);
    var cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    return cloudinary;
});

// Sua abstração de storage
builder.Services.AddScoped<ICloudStorage, CloudinaryStorage>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Evento personalizado para tratar falhas de autenticação
    x.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Previne o comportamento padrão
            context.HandleResponse();

            // Define o status code
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            // Resposta personalizada
            var response = new
            {
                Sucesso = false,
                Mensagem = "Você precisa estar autenticado para acessar este método. Por favor, realize o login no sistema."
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    };
});

builder.Services.Configure<JwtSettingsModel>(builder.Configuration.GetSection("Jwt"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard(); // Se quiser dashboard do Hangfire

// Agendamento de job com Hangfire
RecurringJob.AddOrUpdate<BancoHorasJob>(
    "processar-banco-horas-diario",
    job => job.ExecutarProcessamentoDiario(),
    "30 20 * * *" // Cron: todo dia às 20:30
);

app.Run();