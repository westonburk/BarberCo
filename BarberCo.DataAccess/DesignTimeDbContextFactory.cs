using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // Match the runtime configuration in BarberCo.Api/Program.cs so the
            // design-time model (and migration snapshot) reflect how the app actually runs.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BarberCo.Api"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("Default");
            builder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

            return new DataContext(builder.Options);
        }
    }
}
