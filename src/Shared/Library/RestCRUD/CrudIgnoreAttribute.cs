namespace RestCRUD
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CrudIgnoreAttribute : Attribute
    {

    }
}