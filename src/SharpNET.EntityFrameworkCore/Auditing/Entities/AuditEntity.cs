using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.Auditing.Entities
{
    public abstract class AuditEntity : IAuditEntity
    {
        public abstract DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedById { get; set; }
        public AuditChangeType ChangeType { get; set; }
    }
}
