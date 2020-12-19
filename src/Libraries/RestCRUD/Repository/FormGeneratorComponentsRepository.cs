namespace RestCRUD.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using RestCRUD.Core.Repository;

    public class FormGeneratorComponentsRepository : IFormGeneratorComponentsRepository
    {
        protected Dictionary<Type, Type> _ComponentDict = new Dictionary<Type, Type>();

        public Type _DefaultComponent { get; protected set; }

        public FormGeneratorComponentsRepository()
        {

        }

        public FormGeneratorComponentsRepository(Dictionary<Type, Type> componentRegistrations)
        {
            _ComponentDict = componentRegistrations;
        }
        public FormGeneratorComponentsRepository(Dictionary<Type, Type> componentRegistrations, Type defaultComponent)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
        }

        protected void RegisterComponent(Type key, Type component)
        {
            _ComponentDict.Add(key, component);
        }

        protected void RemoveComponent(Type key)
        {
            _ComponentDict.Remove(key);
        }

        protected virtual Type GetComponent(Type key)
        {
            Type type = key;
            // When the type is an ENUM use Enum as type instead of property
            if (key.IsEnum)
            {
                type = typeof(Enum);
            }
            // When the type is a ValuesReferences use the base type. example ValuesReferences<bool>
            else if (key.BaseType == typeof(ValueReferences))
            {
                type = typeof(ValueReferences);
            }
            // When it's a Nullable type use the underlying type for matching
            else if (Nullable.GetUnderlyingType(key) != null)
            {
                type = Nullable.GetUnderlyingType(key);
            }

            var found = _ComponentDict.TryGetValue(type, out Type outVar);

            return found ? outVar : _DefaultComponent;
        }

        public void Clear()
        {
            _ComponentDict.Clear();
        }

        public void RegisterComponent(object key, Type component)
        {
            RegisterComponent((Type)key, component);
        }

        public void RemoveComponent(object key)
        {
            RemoveComponent((Type)key);
        }

        public Type GetComponent(object key)
        {
            return GetComponent((Type)key);
        }
    }
}
