namespace MagicOperations.Schemas
{
    using System;
    using System.Reflection;
    using MagicModels.Extensions;
    using MagicOperations.Attributes;
    using MagicOperations.Components;

    /// <summary>
    /// Known types helpers.
    /// </summary>
    public static class KnownTypesHelpers
    {
        public static bool IsOperationRenderer(Type operationType)
        {
            return operationType.IsSubclassOfGeneric(typeof(OperationRenderer<,>));
        }

        /// <summary>
        /// Checks whether type is a valid operation class. Every operation class is a valid renderable class.
        /// </summary>
        public static bool IsOperationType(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return false;
            }

            return type.IsDefined(typeof(OperationAttribute));
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsLoginOperationType(Type type)
        {
            return type.IsDefined(typeof(LoginOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsCreateOperationType(Type type)
        {
            return type.IsDefined(typeof(CreateOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid update operation.
        /// </summary>
        public static bool IsUpdateOperationType(Type type)
        {
            return type.IsDefined(typeof(UpdateOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid delete operation.
        /// </summary>
        public static bool IsDeleteOperationType(Type type)
        {
            return type.IsDefined(typeof(DeleteOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsGetDetailsOperationType(Type type)
        {
            return type.IsDefined(typeof(DetailsOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsGetListOperationType(Type type)
        {
            return type.IsDefined(typeof(GetAllOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsGetPagedListOperationType(Type type)
        {
            return type.IsDefined(typeof(GetPagedOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
    }
}
