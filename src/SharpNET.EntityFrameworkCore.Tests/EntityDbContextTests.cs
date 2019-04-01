using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using SharpNET.EntityFrameworkCore.Tests.Data;
using Xunit;

namespace SharpNET.EntityFrameworkCore.Tests
{
    public class EntityDbContextTests
    {
        private DbContextOptions<TestEntityDbContext> CreateOptions()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestEntityDbContext>()
                .UseSqlite(connection)
                .Options;

            return options;
        }

        private void CloseConnection(DbContextOptions options)
        {
            options.GetExtension<SqliteOptionsExtension>()
                    .Connection.Close();
        }

        [Fact]
        public void TestDateCreated()
        {
            var start = DateTime.UtcNow;
            var options = CreateOptions();
            try
            {

                using (var context = new TestEntityDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert a record
                using (var context = new TestEntityDbContext(options))
                {
                    var user = new UserProfileEntity()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "some@email.com"
                    };

                    context.Profiles.Add(user);
                    context.SaveChanges();
                }

                // Use a fresh context to check results
                using (var context = new TestEntityDbContext(options))
                {
                    var user = context.Profiles.First();
                    Assert.True(user.CreatedOn > start);
                    Assert.True(user.CreatedOn == user.ModifiedOn, "CreatedOn and ModifiedOn match");
                }
            }
            finally
            {
                CloseConnection(options);
            }
        }

        [Fact]
        public void TestDateUpdated()
        {
            var start = DateTime.UtcNow;
            var options = CreateOptions();
            try
            {
                using (var context = new TestEntityDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert a record
                var createdOn = DateTime.UtcNow.AddDays(-1);
                using (var context = new TestEntityDbContext(options))
                {
                    var user = new UserProfileEntity()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "some@email.com"
                    };

                    context.Profiles.Add(user);
                    context.SaveChanges();
                    createdOn = user.CreatedOn;

                    // Simulate that the user was added yesterday
                    context.Database.ExecuteSqlCommand(
                        "update Profiles set CreatedOn = {0}, ModifiedOn = {0}",
                        createdOn);
                }

                // Modify the record
                using (var context = new TestEntityDbContext(options))
                {
                    var user = context.Profiles.First();
                    user.FirstName = "ModifiedFirst";
                    context.SaveChanges();
                }

                // Use a fresh context to check results
                using (var context = new TestEntityDbContext(options))
                {
                    var user = context.Profiles.First();
                    Assert.True(user.ModifiedOn > start);
                    Assert.True(user.CreatedOn == createdOn, 
                        $"CreatedOn ({user.CreatedOn}) was not updated, expected ({createdOn})");
                }
            }
            finally
            {
                CloseConnection(options);
            }
        }

        [Fact]
        public void TestSoftDeletes()
        {
            var start = DateTime.UtcNow;
            var options = CreateOptions();
            try
            {
                using (var context = new TestEntityDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert a record
                var createdOn = DateTime.MinValue;
                using (var context = new TestEntityDbContext(options))
                {
                    var user = new UserProfileEntity()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "some@email.com"
                    };

                    context.Profiles.Add(user);
                    context.SaveChanges();
                    createdOn = user.CreatedOn;
                }

                // Delete the record
                using (var context = new TestEntityDbContext(options))
                {
                    var user = context.Profiles.First();
                    context.Profiles.Remove(user);
                    context.SaveChanges();
                }

                // Use a fresh context to check results
                using (var context = new TestEntityDbContext(options))
                {
                    var user = context.Profiles.FirstOrDefault();
                    Assert.NotNull(user);
                    Assert.True(user.IsDeleted);
                    Assert.True(user.DeletedOn.HasValue);
                    Assert.True(user.DeletedOn.Value > start);
                    Assert.True(user.DeletedOn.Value == user.ModifiedOn, $"DeletedOn ({user.DeletedOn}) and ModifiedOn ({user.ModifiedOn})match");
                    Assert.False(string.IsNullOrEmpty(user.DeletedBy));
                    Assert.True(user.CreatedOn == createdOn, "CreatedOn was not updated");
                }
            }
            finally
            {
                CloseConnection(options);
            }
        }
    }
}
