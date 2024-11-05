using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjekPklInventaris.Models;

namespace ProjekPklInventaris.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Tambahkan properti tambahan jika diperlukan
    }

    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Kategori> Kategori { get; set; } = default!;
        public DbSet<Pemasok> Pemasok { get; set; } = default!;
        public DbSet<Barang> Barang { get; set; } = default!;

    }
}
