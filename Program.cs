using GerenciamentoSalas.API.Common.Repositorie;
using GerenciamentoSalas.API.Common.Repositorie.interfaces;
using GerenciamentoSalas.API.Common.Service;
using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Logging
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

//Services
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAgendamentoService,AgendamentoService>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<CommonRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<GerenciamentoContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

var app = builder.Build();

//Scallar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Gerenciamento Salas API")
            .WithTheme(ScalarTheme.Kepler);
    });
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
