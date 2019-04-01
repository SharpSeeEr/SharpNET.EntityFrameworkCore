using System;
using System.Collections.Generic;
using System.Text;
using SharpNET.EntityFrameworkCore.Entities;

namespace SharpNET.EntityFrameworkCore.Tests.Data
{
    class UserProfileEntity : SoftDeleteEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
