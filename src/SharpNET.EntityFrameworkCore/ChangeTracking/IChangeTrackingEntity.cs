using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.ChangeTracking
{
    public interface IChangeTrackingEntity : IEntity
    {
        DateTime Created { get; set; }
        int CreatedById { get; set; }
        DateTime Modified { get; set; }
        int ModifiedById { get; set; }
    }
}
