using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.EntityFrameworkCore.ChangeTracking
{
    public abstract class ChangeTrackingEntity : IChangeTrackingEntity
    {
        protected DateTime _created;
        public DateTime Created => _created;

        protected int _createdById;
        public int CreatedById => _createdById;

        protected DateTime _modified;
        public DateTime Modified => _modified;

        protected int _modifiedById;
        public int ModifiedById => _modifiedById;

        public int Id { get; set; }
    }
}
