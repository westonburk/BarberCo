using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarberCo.DataAccess
{
    public class DataContext : IdentityDbContext<Barber>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Hour> Hours { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map ASP.NET Identity to existing lower-case tables
            builder.Entity<Barber>().ToTable("aspnetusers");
            builder.Entity<IdentityRole>().ToTable("aspnetroles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("aspnetuserclaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("aspnetuserlogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("aspnetusertokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("aspnetroleclaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("aspnetuserroles");
        }
    }
}
