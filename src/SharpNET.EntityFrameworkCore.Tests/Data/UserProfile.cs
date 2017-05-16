using System;
using System.Collections.Generic;
using System.Text;
using SharpNET.EntityFrameworkCore.ChangeTracking;

namespace SharpNET.EntityFrameworkCore.Tests.Data
{
    class UserProfile : IChangeTrackingEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        protected DateTime _created;
        public DateTime Created => _created;

        protected int _createdById;
        public int CreatedById => _createdById;

        protected DateTime _modified;
        public DateTime Modified => _modified;

        protected int _modifiedById;
        public int ModifiedById => _modifiedById;
    }
}
