using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.EntityFrameworkCore.Entities
{
    public interface ISoftDeleteEntity : IEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
        string DeletedBy { get; set; }
    }

    public interface ISoftDeleteEntity<T> : IEntity<T>
    {
    }
}
