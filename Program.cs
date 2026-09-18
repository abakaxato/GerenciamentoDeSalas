using GerenciamentoSalas.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<GerenciamentoContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("ConexãoPadrão")));

var app = builder.Build();

app.UseDefaultFiles(); // Procura automaticamente por index.html na raiz do wwwroot
app.UseStaticFiles();  // Permite servir os arquivos estáticos (.html, .css, .js)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
