namespace VxFormGenerator.Settings.Plain
{
    using System;
    using VxFormGenerator.Core;
    using VxFormGenerator.Form;

    public class VxFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public VxFormOptions()
        {
            FormElementComponent = typeof(FormElement<>);
        }
    }
}
