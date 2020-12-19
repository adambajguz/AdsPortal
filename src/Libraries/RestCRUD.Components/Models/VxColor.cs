namespace RestCRUD.Models
{
    using System.ComponentModel;
    using RestCRUD.Utils;

    [TypeConverter(typeof(StringToVxColorConverter))]
    public class VxColor
    {

        // will contain standard 32bit sRGB (ARGB)
        //
        public string Value { get; private set; }

        public VxColor(string value)
        {
            Value = value;
        }
    }
}
