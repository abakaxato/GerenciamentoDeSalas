using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Logging
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

//Repositorys
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

//Services
builder.Services.AddScoped<IAgendamentoService,AgendamentoService>();

//Controllers
builder.Services.AddControllers();

//Contexts
builder.Services.AddDbContext<GerenciamentoContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
