﻿namespace AdsPortal.Persistence.UoW.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Application.Interfaces.Persistence.UoW.Generic;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Domain.Abstractions.Audit;
    using AdsPortal.Domain.Abstractions.Enums;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Infrastructure.Extensions;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.Persistence.Repository;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Newtonsoft.Json;

    public abstract class GenericAuditableRelationalUnitOfWork : GenericRelationalUnitOfWork, IGenericAuditableRelationalUnitOfWork
    {
        //Audit
        private readonly Lazy<IEntityAuditLogsRepository> _entityAuditLogs;
        public IEntityAuditLogsRepository EntityAuditLogs => _entityAuditLogs.Value;

        protected GenericAuditableRelationalUnitOfWork(ICurrentUserService currentUserService, IGenericRelationalDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {
            _entityAuditLogs = new Lazy<IEntityAuditLogsRepository>(() => GetSpecificRepository<IEntityAuditLogsRepository, EntityAuditLogsRepository>());
        }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();

            return Provider.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();

            return await Provider.SaveChangesAsync(cancellationToken);
        }

        public int SaveChangesWithoutAudit()
        {
            return Provider.SaveChanges();
        }

        public async Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default)
        {
            return await Provider.SaveChangesAsync(cancellationToken);
        }

        //https://www.meziantou.net/entity-framework-core-history-audit-table.htm
        private void OnBeforeSaveChanges()
        {
            if (!Provider.ChangeTracker.AutoDetectChangesEnabled)
                Provider.ChangeTracker.DetectChanges();

            IEnumerable<EntityEntry> entries = Provider.ChangeTracker.Entries();

            IEnumerable<EntityAuditLog> auditLogsToAdd = ProcessChanges(Provider, entries);
            EntityAuditLogs.AddMultiple(auditLogsToAdd);
        }

        #region Helpers
        private static IEnumerable<EntityAuditLog> ProcessChanges(DbContext context, IEnumerable<EntityEntry> entries)
        {
            List<EntityAuditLog> auditLogsToAdd = new List<EntityAuditLog>();

            foreach (EntityEntry entry in entries)
            {
                object entity = entry.Entity;
                EntityState state = entry.State;

                if (state == EntityState.Detached || state == EntityState.Unchanged || entity is IAuditLog || IsEntityIgnored(entity))
                    continue;

                EntityAuditLog auditEntry = new EntityAuditLog
                {
                    TableName = context.Model.GetTableName(entity.GetType()),
                    Action = state switch
                    {
                        EntityState.Added => AuditActions.Added,
                        EntityState.Deleted => AuditActions.Deleted,
                        EntityState.Modified => AuditActions.Modified,
                        _ => throw new NotImplementedException()
                    },
                };
                auditEntry.Values = GetChangedProperties(entry, auditEntry) is Dictionary<string, object> v ? JsonConvert.SerializeObject(v) : null;

                auditLogsToAdd.Add(auditEntry);
            }

            return auditLogsToAdd;
        }

        private static Dictionary<string, object>? GetChangedProperties(EntityEntry entry, EntityAuditLog auditEntry)
        {
            AuditActions action = auditEntry.Action;
            if (action == AuditActions.Deleted)
                return null;

            Dictionary<string, object> newValues = new Dictionary<string, object>();

            foreach (PropertyEntry property in entry.Properties)
            {
                IProperty metadata = property.Metadata;
                if (metadata.IsPrimaryKey())
                {
                    auditEntry.Key = (Guid)property.CurrentValue;

                    if (action == AuditActions.Added)
                        newValues[metadata.Name] = property.CurrentValue;

                    continue;
                }

                if ((property.IsModified || action == AuditActions.Added) && !IsPropertyIgnored(metadata))
                {
                    newValues[metadata.Name] = property.CurrentValue;
                }
            }

            return newValues.Count == 0 ? null : newValues;
        }

        private static bool IsEntityIgnored(object entity)
        {
            Type type = entity.GetType();

            return type.GetCustomAttributes(typeof(AuditIgnoreAttribute), true)
                       .Any() ||
                   type.GetInterfaces()
                       .FirstOrDefault(x => x.GetCustomAttributes(typeof(AuditIgnoreAttribute), false).Any()) != null;
        }

        private static bool IsPropertyIgnored(IProperty metadata)
        {
            return metadata.PropertyInfo
                           .GetCustomAttributes(typeof(AuditIgnoreAttribute), true)
                           .Any() ||
                   metadata.PropertyInfo.HasAttribute<AuditIgnoreAttribute>();
        }

        #endregion
    }
}