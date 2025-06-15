using Application.Interfaces;
using Application.Jobs;
using Application.Services;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Hangfire;
using Hangfire.MySql;
using Hangfire.Storage;

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
builder.Services.AddSwaggerGen();

//Login
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

//Feriado e ferias
builder.Services.AddScoped<IFeriadoService, FeriadoService>();
builder.Services.AddScoped<IFeriadoRepository, FeriadoRepository>();

//Calendario 
builder.Services.AddScoped<ICalendarioService, CalendarioService>();

//Feriado e ferias
builder.Services.AddScoped<IComunicadoService, ComunicadoService>();
builder.Services.AddScoped<IComunicadoRepository,ComunicadoRepository>();

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
                         // Prefixo das tabelas se quiser separar visualmente
                         TablesPrefix = "Hangfire"
                     }));
});

builder.Services.AddHangfireServer();

// Adicione o serviço do job
builder.Services.AddScoped<BancoHorasJob>();


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

app.UseHttpsRedirection();

// USA O CORS
app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard(); // Opcional: expõe a dashboard no /hangfire

RecurringJob.AddOrUpdate<BancoHorasJob>(
    "processar-banco-horas-diario",
    job => job.ExecutarProcessamentoDiario(),
    "30 20 * * *" // Cron: todo dia às 
);

app.Run();
