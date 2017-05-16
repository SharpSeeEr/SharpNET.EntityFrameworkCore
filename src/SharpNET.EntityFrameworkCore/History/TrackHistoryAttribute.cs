using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.EntityFrameworkCore.History
{
    public class TrackHistoryAttribute : Attribute
    {
        //public Type AuditEntityTypeName { get; set; }

        public TrackHistoryAttribute() //Type auditEntityTypeName)
        {
            //AuditEntityTypeName = auditEntityTypeName;
        }
    }
}
