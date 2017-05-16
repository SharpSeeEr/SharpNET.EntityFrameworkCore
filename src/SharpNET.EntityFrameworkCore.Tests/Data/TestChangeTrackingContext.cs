using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharpNET.EntityFrameworkCore.ChangeTracking;
using SharpNET.EntityFrameworkCore.Extensions;

namespace SharpNET.EntityFrameworkCore.Tests.Data
{
    class TestChangeTrackingContext : ChangeTrackingDbContext
    {
        public TestChangeTrackingContext(DbContextOptions options)
            : base(options)
        {
            _userId = 1;
        }

        public DbSet<UserProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasChangeTracking();

            base.OnModelCreating(modelBuilder);
        }
    }
}
