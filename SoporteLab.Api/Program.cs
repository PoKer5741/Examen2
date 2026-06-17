using Microsoft.EntityFrameworkCore;
using SoporteLab.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar la base de datos SQLite
builder.Services.AddDbContext<SoporteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Activar el uso de Controladores (ESTO FALTABA)
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// 3. Mapear las rutas de los Controladores (ESTO FALTABA)
app.MapControllers();

app.Run();

