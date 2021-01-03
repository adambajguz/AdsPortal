﻿namespace AdsPortal.Application.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public string EntityName { get; }
        public Guid? Id { get; }

        public NotFoundException(string entityName)
            : base($"Entity '{entityName}' was not found.")
        {
            EntityName = entityName;
        }

        public NotFoundException(string entityName, Exception innerException)
            : base($"Entity '{entityName}' was not found.", innerException)
        {
            EntityName = entityName;
        }

        public NotFoundException(string entityName, Guid id)
            : base($"Entity '{entityName}' ({id}) was not found.")
        {
            EntityName = entityName;
            Id = id;
        }

        public NotFoundException(string entityName, Guid id, Exception innerException)
            : base($"Entity '{entityName}' ({id}) was not found.", innerException)
        {
            EntityName = entityName;
            Id = id;
        }
    }
}