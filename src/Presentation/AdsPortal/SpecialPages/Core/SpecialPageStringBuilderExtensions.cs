namespace AdsPortal.SpecialPages.Core
{
    using System.Text;

    public static class SpecialPageStringBuilderExtensions
    {
        public static StringBuilder BeginHTML(this StringBuilder sb)
        {
            return sb.Append("<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body>");
        }

        public static StringBuilder EndHTML(this StringBuilder sb)
        {
            return sb.Append("</body></html>");
        }
    }
}
