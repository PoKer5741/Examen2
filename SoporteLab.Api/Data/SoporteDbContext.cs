using Microsoft.EntityFrameworkCore;
using SoporteLab.Api.Models;

namespace SoporteLab.Api.Data
{
    public class SoporteDbContext : DbContext
    {
        public SoporteDbContext(DbContextOptions<SoporteDbContext> options) : base(options) { }

        // TABLA AQUI !!!!!!!!!!!!!!
        public DbSet<Ticket> Tickets { get; set; }
    }
}