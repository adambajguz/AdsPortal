namespace MagicOperations.Schemas
{
    using System;
    using System.Reflection;
    using MagicOperations.Attributes;

    /// <summary>
    /// Known types helpers.
    /// </summary>
    public static class KnownTypesHelpers
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static bool IsOperationCommandType(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
                return false;

            return type.IsDefined(typeof(CreateOperationAttribute)) ||
                   type.IsDefined(typeof(UpdateOperationAttribute)) ||
                   type.IsDefined(typeof(DeleteOperationAttribute)) ||
                   type.IsDefined(typeof(GetDetailsOperationAttribute)) ||
                   type.IsDefined(typeof(GetListOperationAttribute)) ||
                   type.IsDefined(typeof(GetPagedListOperationAttribute));
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
            return type.IsDefined(typeof(GetDetailsOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsGetListOperationType(Type type)
        {
            return type.IsDefined(typeof(GetListOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid create operation.
        /// </summary>
        public static bool IsGetPagedListOperationType(Type type)
        {
            return type.IsDefined(typeof(GetPagedListOperationAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
    }
}
