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

// CONFIGURAÇÃO CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

// 3) Sempre exibe página de erro detalhada
app.UseDeveloperExceptionPage();

// 4) Sempre habilita Swagger / SwaggerUI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // aponta para o swagger.json v1
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TDE Evolucionario API V1");
    // opcional: expõe o Swagger UI na raiz (/)
    c.RoutePrefix = string.Empty;
});

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
