namespace RestCRUD.Settings.Plain
{
    using System;
    using RestCRUD.Core;
    using RestCRUD.Form;

    public class VxFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public VxFormOptions()
        {
            FormElementComponent = typeof(FormElement<>);
        }
    }
}
