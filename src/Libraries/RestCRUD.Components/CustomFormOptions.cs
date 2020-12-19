namespace RestCRUD.Components
{
    using System;
    using RestCRUD.Core;

    public class CustomFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public CustomFormOptions()
        {
            FormElementComponent = typeof(FormElement<>);
        }
    }
}
