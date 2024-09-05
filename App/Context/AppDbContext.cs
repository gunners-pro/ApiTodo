using ApiTodo.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.App.Context;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Todo> Todos { get; set; }
}
