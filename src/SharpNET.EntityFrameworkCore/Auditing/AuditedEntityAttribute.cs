using System;
using System.Collections.Generic;
using System.Text;

namespace SharpNET.Core.EF.Auditing
{
    public class TrackHistoryAttribute : Attribute
    {
        public Type AuditEntityTypeName { get; set; }

        public TrackHistoryAttribute(Type auditEntityTypeName)
        {
            AuditEntityTypeName = auditEntityTypeName;
        }
    }
}
