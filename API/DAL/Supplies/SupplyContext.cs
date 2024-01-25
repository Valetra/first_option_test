using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace DAL.Contexts;

public class SupplyContext(DbContextOptions<SupplyContext> options) : DbContext(options)
{
    public DbSet<Supply> Supplies { get; set; }
}