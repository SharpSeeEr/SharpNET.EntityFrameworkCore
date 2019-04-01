using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.Entities
{
    public abstract class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public abstract string GetUsername();

        public override int SaveChanges()
        {
            UpdateEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        protected void UpdateEntities()
        {
            // Create the changeDate here so CreatedOn, ModifiedOn, and DeletedOn all match exactly
            var changeDate = DateTime.UtcNow;
            UpdateDates(changeDate);
            UpdateSoftDeletes(changeDate);
        }

        /// <summary>
        /// Automatically sets Created and Modified dates and by username
        /// </summary>
        protected void UpdateDates(DateTime changeDate)
        {
            var entries = ChangeTracker.Entries<IEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = changeDate;
                    entry.Entity.CreatedBy = GetUsername();
                }

                // Modified always gets updated, even for inserts
                entry.Entity.ModifiedOn = changeDate;
                entry.Entity.ModifiedBy = GetUsername();
            }
        }

        /// <summary>
        /// Converts deletions to updates and sets the DeletedOn flag, date, and user
        /// </summary>
        /// <param name="changeDate"></param>
        protected void UpdateSoftDeletes(DateTime changeDate)
        {
            var entries = ChangeTracker.Entries<ISoftDeleteEntity>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.Entity.ModifiedOn = changeDate;
                entry.Entity.ModifiedBy = GetUsername();
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedOn = changeDate;
                entry.Entity.DeletedBy = GetUsername();
                entry.State = EntityState.Modified;
            }
        }
    }
}
