using ApiTodo.App.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlServer");

{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
}

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
