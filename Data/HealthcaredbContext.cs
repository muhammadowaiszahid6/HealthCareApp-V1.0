using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HealthcareApp.Models.healthcaredb;

namespace HealthcareApp.Data
{
    public partial class healthcaredbContext : DbContext
    {
        public healthcaredbContext()
        {
        }

        public healthcaredbContext(DbContextOptions<healthcaredbContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<HealthcareApp.Models.healthcaredb.Organization> Organizations { get; set; }

        public DbSet<HealthcareApp.Models.healthcaredb.Speciality> Specialities { get; set; }

        public DbSet<HealthcareApp.Models.healthcaredb.Withdraw> Withdraws { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}