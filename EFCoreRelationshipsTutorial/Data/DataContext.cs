using EFCoreRelationshipsTutorial.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsTutorial.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        //public DbSet<Base> Bases { get; set; }
        public DbSet<WhatsApp> WhatsApps { get; set; }
        public DbSet<Telegram> Telegram { get; set; }
    }
}
