namespace MagicModels.Components
{
    using System;
    using System.Reflection;
    using MagicModels.Internal.Extensions;
    using MagicModels.Schemas;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public abstract class PropertyRenderer<T> : ComponentBase, IPropertyRenderer
    {
        private Func<T>? _getter;
        private Action<T?>? _setter;

        public T? Value
        {
            get
            {
                _getter ??= (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), Model, GetGetter());

                return _getter();
            }

            set
            {
                _setter ??= (Action<T?>)Delegate.CreateDelegate(typeof(Action<T?>), Model, GetSetter());

                _setter(value);
            }
        }

        object? IPropertyRenderer.Value => Value;

        [Parameter] public object Model { get; init; } = default!;

        [Parameter] public object? Context { get; init; }

        [Parameter] public RenderablePropertySchema PropertySchema { get; init; } = default!;

        [Parameter] public bool IsWrite { get; init; }

        [Inject] private ILogger<PropertyRenderer<T>> Logger { get; init; } = default!;

        private MethodInfo GetGetter()
        {
            MethodInfo? methodInfo = PropertySchema.Property.GetGetMethod();

            if (methodInfo is null)
            {
                Logger.LogCritical("No public getter available for property {PropertyName} in type {Type}.", PropertySchema.Property.Name, typeof(T));
                throw new MethodAccessException($"No public getter available for property '{PropertySchema.Property.Name}' in type '{typeof(T).FullName}'.");
            }

            return methodInfo;
        }

        private MethodInfo GetSetter()
        {
            PropertyInfo? propertyInfo = !PropertySchema.Property.CanWrite || PropertySchema.Property.IsInitOnly() ? null : PropertySchema.Property;
            MethodInfo? methodInfo = propertyInfo?.GetSetMethod();

            if (methodInfo is null)
            {
                Logger.LogCritical("No public setter available for property {PropertyName} in type {Type}.", PropertySchema.Property.Name, typeof(T));
                throw new MethodAccessException($"No public setter available for property '{PropertySchema.Property.Name}' in type '{typeof(T).FullName}'.");
            }

            return methodInfo;
        }
    }
}
