using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNET.EntityFrameworkCore.ChangeTracking
{
    public interface IChangeTrackingEntity : IEntity
    {
        DateTime Created { get; }
        int CreatedById { get; }
        DateTime Modified { get; }
        int ModifiedById { get; }
    }
}
