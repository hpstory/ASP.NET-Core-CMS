using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Entities
{
    public class BlogDbContext : DbContext
    {
        public static readonly LoggerFactory loggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
