using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace DAL.Contexts;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
}