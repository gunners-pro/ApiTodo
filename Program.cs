using ApiTodo.App.Context;
using ApiTodo.App.Repositories.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlServer");

{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}

var app = builder.Build();

{
    app.MapControllers();
}

app.MapGet("/", () => "Hello World!");

app.Run();
