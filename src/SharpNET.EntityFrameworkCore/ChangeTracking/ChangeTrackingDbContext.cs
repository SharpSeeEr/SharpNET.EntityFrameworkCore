using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public Task<int> SaveChangesAsync()
        {
            UpdateDates();
            return base.SaveChangesAsync();
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
            // Have to set the values using the CurrentValues collection because the setters may/should not be accessible/defined.
            if (auditedEntry.State == EntityState.Added)
            {
                auditedEntry.CurrentValues["CreatedById"] = _userId;
                auditedEntry.CurrentValues["Created"] = changeDate;
            }

            // Modified always gets updated
            auditedEntry.CurrentValues["ModifiedById"] = _userId;
            auditedEntry.CurrentValues["Modified"] = changeDate;
        }
    }
}
