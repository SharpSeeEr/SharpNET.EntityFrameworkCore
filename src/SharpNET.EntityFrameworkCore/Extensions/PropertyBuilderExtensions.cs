using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharpNET.EntityFrameworkCore.ChangeTracking;

namespace SharpNET.EntityFrameworkCore.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static EntityTypeBuilder<T> HasChangeTracking<T>(this EntityTypeBuilder<T> entityBuilder) where T : class, IChangeTrackingEntity
        {
            entityBuilder.Property(e => e.Created).HasMatchingField();
            entityBuilder.Property(e => e.CreatedById).HasMatchingField();
            entityBuilder.Property(e => e.Modified).HasMatchingField();
            entityBuilder.Property(e => e.ModifiedById).HasMatchingField();
            return entityBuilder;
        }

        public static PropertyBuilder<T> HasMatchingField<T>(this PropertyBuilder<T> propBuilder)
        {
            propBuilder.HasField(GetBackingFieldName(propBuilder.Metadata.Name));
            return propBuilder;
        }

        private static string GetBackingFieldName(string propName)
        {
            var fieldName = "_" + propName[0].ToString().ToLower() + propName.Substring(1);
            return fieldName;
        }
    }
}
