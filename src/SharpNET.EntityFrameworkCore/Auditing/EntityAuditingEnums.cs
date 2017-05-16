using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.Auditing
{
    public enum AuditChangeType
    {
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}
