namespace MagicOperations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Configurations;
    using MagicOperations.Extensions;
    using MagicOperations.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class MagicOperationsBuilder
    {
        private string? _baseUri;
        private readonly List<Type> _operationTypes = new List<Type>();

        /// <summary>
        /// Initializes an instance of <see cref="MagicOperationsBuilder"/>.
        /// </summary>
        public MagicOperationsBuilder()
        {

        }

        public MagicOperationsBuilder UseBaseUri(string baseUri)
        {
            _baseUri = baseUri;

            return this;
        }

        #region Operations
        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperation(Type operationType)
        {
            if (!KnownTypesHelpers.IsOperationCommandType(operationType))
                throw new MagicOperationsException($"{operationType.FullName} is not a valid operation type.");

            _operationTypes.Add(operationType);

            return this;
        }

        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperation<T>()
            where T : class
        {
            return AddOperation(typeof(T));
        }

        /// <summary>
        /// Adds multiple operations to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperations(IEnumerable<Type> operationTypes)
        {
            foreach (Type commandType in operationTypes)
                AddOperation(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(Assembly operationAssembly)
        {
            foreach (Type commandType in operationAssembly.ExportedTypes.Where(KnownTypesHelpers.IsOperationCommandType))
                AddOperation(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assemblies to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly commandAssembly in operationAssemblies)
                AddOperationsFrom(commandAssembly);

            return this;
        }

        /// <summary>
        /// Adds operations from the calling assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFromThisAssembly()
        {
            return AddOperationsFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        /// <summary>
        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
        /// Default values are used in place of parameters that were not specified.
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </summary>
        public MagicOperationsConfiguration Build()
        {
            _ = _baseUri ?? throw new MagicOperationsException($"Base URI not set.");

            if (_operationTypes.Count == 0)
                throw new MagicOperationsException("At least one operation must be defined in the application.");

            var resolvedOperations = OperationSchemaResolver.Resolve(_operationTypes);
            MagicOperationsConfiguration configuration = new(_baseUri, _operationTypes, resolvedOperations.Groups, resolvedOperations.Schemas);

            return configuration;
        }
    }
}
