using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Entities
{
    public class CMSDbContext : DbContext
    {
        public CMSDbContext(DbContextOptions<CMSDbContext> options) : base(options) { }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comments>()
                .HasOne(x => x.Articles)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ArticleID)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
