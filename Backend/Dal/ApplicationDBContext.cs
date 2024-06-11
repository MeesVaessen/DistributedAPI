using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dal.Interfaces;
using Dal.Models;

namespace Dal
{
        public class ApplicationDBContext : DbContext, IDBContext
        {
        protected string _connectionString; 

        public ApplicationDBContext(IConfiguration config, ILogger<ApplicationDBContext> logger)
        {
            string? connectionString = config.GetConnectionString("DatabaseConnection");
            if (connectionString == null || connectionString == "")
            {
                logger.LogWarning("Connection string is missing or empty in the appsettings.json file");
                _connectionString = "";
            }
            else
                _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(_connectionString);
    }
}