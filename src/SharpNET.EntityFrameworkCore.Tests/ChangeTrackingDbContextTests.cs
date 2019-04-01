using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using SharpNET.EntityFrameworkCore.Tests.Data;
using Xunit;

namespace SharpNET.EntityFrameworkCore.Tests
{
    public class ChangeTrackingDbContextTests
    {
        private DbContextOptions<TestChangeTrackingContext> CreateOptions()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestChangeTrackingContext>()
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
        public void TestTrackingDateCreated()
        {
            var options = CreateOptions();
            try
            {

                using (var context = new TestChangeTrackingContext(options))
                {
                    context.Database.EnsureCreated();
                }

                var start = DateTime.UtcNow;
                using (var context = new TestChangeTrackingContext(options))
                {
                    var user = new UserProfile()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "some@email.com"
                    };

                    context.Add(user);

                    context.SaveChanges();
                }

                using (var context = new TestChangeTrackingContext(options))
                {
                    var user = context.Profiles.Find(1);
                    Assert.True(user.Created > start);
                }
            }
            finally
            {
                CloseConnection(options);
            }
        }

        [Fact]
        public void TestTrackingDateUpdated()
        {
            var options = CreateOptions();
            try
            {
                using (var context = new TestChangeTrackingContext(options))
                {
                    context.Database.EnsureCreated();
                }

                var start = DateTime.UtcNow;
                using (var context = new TestChangeTrackingContext(options))
                {

                    var user = new UserProfile()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "some@email.com"
                    };

                    context.Add(user);

                    context.SaveChanges();

                    context.Database.ExecuteSqlCommand("update Profiles set Created = {0}, Modified = {0}", DateTime.UtcNow.AddDays(-1));
                }

                using (var context = new TestChangeTrackingContext(options))
                {
                    var user = context.Profiles.Find(1);
                    user.FirstName = "Modified";
                    context.SaveChanges();
                }

                using (var context = new TestChangeTrackingContext(options))
                {
                    var user = context.Profiles.Find(1);
                    Assert.True(user.Modified > start);
                }
            }
            finally
            {
                CloseConnection(options);
            }
        }
    }
}
