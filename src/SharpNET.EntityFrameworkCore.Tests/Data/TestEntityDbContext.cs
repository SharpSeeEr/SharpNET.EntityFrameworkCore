using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharpNET.EntityFrameworkCore.Entities;
using SharpNET.EntityFrameworkCore.Extensions;

namespace SharpNET.EntityFrameworkCore.Tests.Data
{
    class TestEntityDbContext : EntityDbContext
    {
        public TestEntityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public override string GetUsername()
        {
            return "TestUser";
        }

        public DbSet<UserProfileEntity> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
