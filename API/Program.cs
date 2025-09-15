// put usings at the very top
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers
builder.Services.AddControllers();

// 2) Register our DbContext (DI)
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

app.UseHttpsRedirection();

// 3) Map controllers and run
app.MapControllers();
app.Run();