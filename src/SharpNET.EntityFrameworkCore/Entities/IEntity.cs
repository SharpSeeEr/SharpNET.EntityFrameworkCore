using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.EntityFrameworkCore.Entities
{
    public interface IModifiableEntity
    {
    }

    public interface IEntity : IModifiableEntity
    {
        object Id { get; }
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        byte[] Version { get; set; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }
}