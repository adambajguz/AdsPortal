namespace MagicOperations.Components
{
    using System;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationPropertyRenderer<T> : ComponentBase
    {
        private Func<T>? _getter;
        private Action<T?>? _setter;

        public T? Value
        {
            get
            {
                _getter ??= (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), Model, PropertySchema.Property.GetGetMethod() ??
                                throw new MethodAccessException($"No public getter available for property '{PropertySchema.Property.Name}' in type '{typeof(T).FullName}'."));

                return _getter();
            }

            set
            {
                _setter ??= (Action<T?>)Delegate.CreateDelegate(typeof(Action<T?>), Model, PropertySchema.Property.GetSetMethod() ??
                                throw new MethodAccessException($"No public setter available for property '{PropertySchema.Property.Name}' in type '{typeof(T).FullName}'."));

                _setter(value);
            }
        }

        [Parameter]
        public object Model { get; init; } = default!;

        [Parameter]
        public RenderablePropertySchema PropertySchema { get; init; } = default!;

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;
    }
}
