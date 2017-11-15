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

        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedById { get; set; }
    }
}
