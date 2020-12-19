namespace VxFormGenerator.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.AspNetCore.Components.Rendering;

    public class RenderFormElements : OwningComponentBase
    {
        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] private EditContext CascadedEditContext { get; set; }

        /// <summary>
        /// Override the default render method, determining if the <see cref="EditContext.Model"/> is a regular class
        /// </summary>
        /// <param name="builder">Instance of the page builder</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            // Check the type of the model
            var modelType = CascadedEditContext.Model.GetType();

            // Look over all the properties in the class.
            // TODO: Should have an option to be excluded from selection
            foreach (var propertyInfo in modelType.GetProperties().Where(w => w.GetCustomAttribute<VxIgnoreAttribute>() == null))
            {
                // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
                MethodInfo method = typeof(RenderFormElements).GetMethod(nameof(RenderFormElements.CreateFormElementReferencePoco), BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo genericMethod = method.MakeGenericMethod(propertyInfo.PropertyType);
                // Execute the method with the following parameters
                genericMethod.Invoke(this, new object[] { CascadedEditContext.Model, propertyInfo, builder });
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private void CreateFormElementReferencePoco<TValue>(object model, PropertyInfo propertyInfo, RenderTreeBuilder builder)
        {
            var valueChanged = Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                        EventCallback.Factory.Create<TValue>(
                            this,
                            EventCallback.Factory.CreateInferred(this, __value => propertyInfo.SetValue(model, __value),
                            (TValue)propertyInfo.GetValue(model))));

            // Create an expression to set the ValueExpression-attribute.
            var constant = Expression.Constant(model, model.GetType());
            var exp = Expression.Property(constant, propertyInfo.Name);
            var lamb = Expression.Lambda<Func<TValue>>(exp);

            var formElementReference = new FormElementReference<TValue>()
            {
                Value = (TValue)propertyInfo.GetValue(model),
                ValueChanged = valueChanged,
                ValueExpression = lamb,
                Key = propertyInfo.Name
            };

            var elementType = typeof(VxFormElementLoader<TValue>);

            builder.OpenComponent(0, elementType);
            builder.AddAttribute(1, nameof(VxFormElementLoader<TValue>.ValueReference), formElementReference);
            builder.CloseComponent();
        }
    }
}
