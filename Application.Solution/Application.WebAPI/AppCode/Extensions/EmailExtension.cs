using System.Text.RegularExpressions;

namespace Application.WebAPI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static bool IsEmail(this string text)
        {
            return Regex.IsMatch(text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        }
    }
}
