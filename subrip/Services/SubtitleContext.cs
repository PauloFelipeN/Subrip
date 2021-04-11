using Microsoft.EntityFrameworkCore;
using subrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace subrip.Services
{
    public class SubtitleContext : DbContext
    {
        public SubtitleContext(DbContextOptions<SubtitleContext> options):base(options)
        {
        }

        public DbSet<Subtitle> Subtitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subtitle>().HasKey(m => m.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
