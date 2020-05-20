using JTSK.Models;
using Microsoft.EntityFrameworkCore;

namespace JTSK
{
    public class ApplicationDbContext : DbContext
    {
        string _path;

        public DbSet<Coordinate> Coordinates { get; set; }

        public ApplicationDbContext(string path)
        {
            _path = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_path}");
        }
    }
}
