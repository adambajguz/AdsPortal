namespace MagicModels.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicModels.Components;
    using MagicModels.Components.Defaults;
    using MagicModels.Components.Defaults.PropertyRenderers;
    using MagicModels.Extensions;
    using MagicModels.Internal.Schemas;
    using MagicModels.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class MagicModelsBuilder
    {
        private readonly Dictionary<Type, Type> _defaultPropertyRenderers = new()
        {
            { typeof(int), typeof(IntRenderer) },
            { typeof(bool), typeof(BoolRenderer) },
            { typeof(DateTime), typeof(DateTimeRenderer) },
            { typeof(string), typeof(StringRenderer) },
            { typeof(IEnumerable), typeof(CollectionRenderer) }
        };

        private readonly List<(Type, Type?, Dictionary<string, RenderablePropertyConfiguration>?)> _renderableClassesTypes = new();

        private Type? _defaultModelRenderer;
        private Type? _anyPropertyRenderer;

        /// <summary>
        /// Initializes an instance of <see cref="MagicModelsBuilder"/>.
        /// </summary>
        public MagicModelsBuilder()
        {

        }

        #region Model renderer
        public MagicModelsBuilder UseModelRenderer(Type modelRender)
        {
            if (!modelRender.IsSubclassOf(typeof(ModelRenderer<>)))
            {
                throw new MagicModelsException($"{modelRender.FullName} is not a valid model renderer type.");
            }

            _defaultModelRenderer = modelRender;

            return this;
        }
        #endregion

        #region Default property renderers
        public MagicModelsBuilder UseDefaultPropertyRenderer(Type propertyType, Type rendererType)
        {
            if (!rendererType.IsSubclassOf(typeof(PropertyRenderer<>).MakeGenericType(propertyType)))
            {
                throw new MagicModelsException($"{rendererType.FullName} is not a valid property renderer type.");
            }

            _defaultPropertyRenderers[propertyType] = rendererType;

            return this;
        }

        public MagicModelsBuilder UseDefaultPropertyRenderer<TPropety, TRenderer>()
            where TRenderer : PropertyRenderer<TPropety>
        {
            return UseDefaultPropertyRenderer(typeof(TPropety), typeof(TRenderer));
        }
        #endregion

        #region Any property renderer
        public MagicModelsBuilder UseAnyPropertyRenderer(Type rendererType)
        {
            if (!rendererType.IsGenericType || !rendererType.IsSubclassOf(typeof(PropertyRenderer<>)))
            {
                throw new MagicModelsException($"{rendererType.FullName} is not a valid any property renderer type.");
            }

            _anyPropertyRenderer = rendererType;

            return this;
        }
        #endregion

        #region Renderable class
        /// <summary>
        /// Adds a renderable class to the application.
        /// Options is a dictionary where the key is a property name.
        /// </summary>
        public MagicModelsBuilder AddRenderableClass(Type type, Type? renderer = null, Dictionary<string, RenderablePropertyConfiguration>? options = null)
        {
            _renderableClassesTypes.Add((type, renderer, options));

            return this;
        }

        /// <summary>
        /// Adds a renderable class to the application.
        /// Options is a dictionary where the key is a property name.
        /// </summary>
        public MagicModelsBuilder AddRenderableClass<TModel>(Type? renderer = null, Dictionary<string, RenderablePropertyConfiguration>? options = null)
            where TModel : class
        {
            return AddRenderableClass(typeof(TModel), renderer, options);
        }

        /// <summary>
        /// Adds a renderable class to the application.
        /// Options is a dictionary where the key is a property name.
        /// </summary>
        public MagicModelsBuilder AddRenderableClass<TModel, TModelRenderer>(Dictionary<string, RenderablePropertyConfiguration>? options = null)
            where TModel : class
        {
            return AddRenderableClass(typeof(TModel), typeof(TModelRenderer), options);
        }

        /// <summary>
        /// Adds renderable classes.
        /// Only adds public valid renderable classes.
        /// Generic classes will not be added, unless fully defined in operation ResponseType or in configuration.
        /// </summary>
        public MagicModelsBuilder AddRenderableClasses(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (!RenderableClassSchema.IsRenderableClassType(type))
                {
                    throw new MagicModelsException($"{type.FullName} is not a valid renderable class type.");
                }

                AddRenderableClass(type);
            }

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the specified assembly to the application.
        /// Only adds public valid renderable classes.
        /// Generic classes will not be added, unless fully defined in operation ResponseType or in configuration.
        /// </summary>
        public MagicModelsBuilder AddRenderableClassesFrom(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes.Where(RenderableClassSchema.IsRenderableClassType))
            {
                _renderableClassesTypes.Add((type, null, null));
            }

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the specified assemblies to the application.
        /// Only adds public valid renderable types classes.
        /// Generic classes will not be added, unless fully defined in operation ResponseType or in configuration.
        /// </summary>
        public MagicModelsBuilder AddRenderableClassesFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly assembly in operationAssemblies)
            {
                AddRenderableClassesFrom(assembly);
            }

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the calling assembly to the application.
        /// Only adds public valid renderable classes.
        /// </summary>
        public MagicModelsBuilder AddRenderableClassesFromThisAssembly()
        {
            return AddRenderableClassesFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        public MagicModelsConfiguration Build()
        {
            if (_renderableClassesTypes.Count == 0)
            {
                throw new MagicModelsException("At least one operation must be defined in the application.");
            }

            _defaultModelRenderer ??= typeof(DefaultModelRenderer<>);
            _anyPropertyRenderer ??= typeof(AnyRenderer<>);

            IReadOnlyDictionary<Type, RenderableClassSchema> resolvedRenderableClasses = RenderableClassSchemaResolver.Resolve(_renderableClassesTypes);

            MagicModelsConfiguration configuration = new(resolvedRenderableClasses,
                                                         _defaultPropertyRenderers,
                                                         _defaultModelRenderer,
                                                         _anyPropertyRenderer);

            return configuration;
        }
    }
}
