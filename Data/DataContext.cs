using Microsoft.EntityFrameworkCore;

namespace ProjekPklInventaris.Models 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Kategori> Kategori { get; set; } = default!;
        public DbSet<Pemasok> Pemasok { get; set; } = default!;
        public DbSet<Barang> Barang { get; set; } = default!;

    }
}
