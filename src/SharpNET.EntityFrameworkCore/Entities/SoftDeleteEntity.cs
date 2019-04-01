using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.EntityFrameworkCore.Entities
{
    public class SoftDeleteEntity<T> : Entity<T>, ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
    }
}
