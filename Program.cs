using ApiTodo.App.Context;
using ApiTodo.App.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ApiTodo.App.Repositories.Todos;
using ApiTodo.App.Security.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlServer");
var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key"); ;

{
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer();
    builder.Services.AddControllers();
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITodoRepository, TodoRepository>();
    builder.Services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(builder.Configuration));
    builder.Services.AddScoped<IAccessTokenValidator>(opt => new JwtTokenValidator(builder.Configuration));
}

var app = builder.Build();

{
    app.MapControllers();
}

app.MapGet("/", () => "Hello World!");

app.Run();
