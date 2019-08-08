using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models
{
    public class DBContext : IdentityDbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>()
                  .HasOne(m => m.FromUser)
                  .WithMany(t => t.FromRequests)
                  .HasForeignKey(m => m.FromUserID)
                  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Request>()
                        .HasOne(m => m.ToUser)
                        .WithMany(t => t.ToRequests)
                        .HasForeignKey(m => m.ToUserID)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
