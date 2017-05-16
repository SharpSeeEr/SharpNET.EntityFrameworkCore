using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using SharpNET.EntityFrameworkCore.Auditing.Entities;
using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace SharpNET.EntityFrameworkCore.ChangeTracking
{
    public class ChangeTrackingDbContext : DbContext
    {
        public ChangeTrackingDbContext(DbContextOptions options)
            :base(options)
        {

        }

        protected int _userId = 0;
        public int UserId { get { return _userId; } }

        public int AuditTypeMap { get; private set; }

        public override int SaveChanges()
        {
            UpdateDates();
            return base.SaveChanges();
        }

        protected void UpdateDates()
        {
            // In On Model Creating, enable auditing for entities
            // When saving, check if any of the entities with auditing enabled are being added or changed
            // For each of those entities, create the audit entity and add to the context
            // How do we get the Id of the entity to put on the AuditEntity?

            // Process any auditable objects.
            var auditedEntries = ChangeTracker.Entries<IChangeTrackingEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            var changeDate = DateTime.UtcNow;

            foreach (var auditedEntry in auditedEntries)
            {
                UpdateEntityDates(changeDate, auditedEntry);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected void UpdateEntityDates(DateTime changeDate, EntityEntry<IChangeTrackingEntity> auditedEntry)
        {
            if (auditedEntry.State == EntityState.Added)
            {
                auditedEntry.CurrentValues["CreatedById"] = _userId;
                auditedEntry.CurrentValues["Created"] = changeDate;
            }

            // Modified always gets updated
            auditedEntry.CurrentValues["ModifiedById"] = _userId;
            auditedEntry.CurrentValues["Modified"] = changeDate;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var changeTrackingType = typeof(ChangeTrackingEntity).GetTypeInfo();
            var entityTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var item in entityTypes.Where(t => changeTrackingType.IsAssignableFrom(t.ClrType.GetTypeInfo())))
            {
                var entity = modelBuilder.Entity(item.ClrType);
                entity.Property("Modified")
                    //.HasField("_modified")
                    .HasValueGenerator<DateTimeNowValueGenerator>()
                    .ValueGeneratedOnAddOrUpdate()
                    .Metadata.IsReadOnlyBeforeSave = false;

                entity.Property("ModifiedById")
                    //.HasField("_modifiedById")
                    .HasValueGenerator<UserIdValueGenerator>()
                    .ValueGeneratedOnAddOrUpdate()
                    .Metadata.IsReadOnlyBeforeSave = false;

                entity.Property("Created")
                    //.HasField("_created")
                    .HasValueGenerator<DateTimeNowValueGenerator>()
                    .ValueGeneratedOnAdd()
                    .Metadata.IsReadOnlyBeforeSave = false;

                entity.Property("CreatedById")
                    //.HasField("_createdById")
                    .HasValueGenerator<UserIdValueGenerator>()
                    .ValueGeneratedOnAdd()
                    .Metadata.IsReadOnlyBeforeSave = false;
            }



            base.OnModelCreating(modelBuilder);
        }
    }

    public class DateTimeNowValueGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTime Next(EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }

    public class UserIdValueGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            return ((ChangeTrackingDbContext)entry.Context).UserId;
        }
    }
}
