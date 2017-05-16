using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.Auditing.Entities
{
    public interface IAuditEntity
    {
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
        int ModifiedById { get; set; }
        AuditChangeType ChangeType { get; set; }
    }
}
