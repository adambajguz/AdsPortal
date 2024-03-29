﻿namespace AdsPortal.WebApi.Application.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public string EntityName { get; } = string.Empty;
        public Guid? Id { get; }

        public NotFoundException(string entityName)
            : base($"{entityName} not found.")
        {
            EntityName = entityName;
        }

        public NotFoundException(string entityName, Exception innerException)
            : base($"{entityName} not found.", innerException)
        {
            EntityName = entityName;
        }

        public NotFoundException(string entityName, Guid id)
            : base($"{entityName} ({id}) not found.")
        {
            EntityName = entityName;
            Id = id;
        }

        public NotFoundException(string entityName, Guid id, Exception innerException)
            : base($"{entityName} ({id}) not found.", innerException)
        {
            EntityName = entityName;
            Id = id;
        }

        public NotFoundException(Guid id)
            : base($"{id} not found.")
        {
            Id = id;
        }

        public NotFoundException(Guid id, Exception innerException)
            : base($"{id} not found.", innerException)
        {
            Id = id;
        }
    }
}
