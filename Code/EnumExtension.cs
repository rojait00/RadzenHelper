using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.Code
{
    public static class EnumExtension
    {
        public static List<Enum> GetEnums<T>(this T enumVal) where T : Enum
        {
            var values = Enum.GetValues(enumVal.GetType());
            return values.Cast<Enum>().ToList();
        }

        public static List<Enum> SplitFlags<T>(this T enumVal) where T : Enum
        {
            var allPermissions = enumVal.GetEnums();
            return allPermissions.Where(x => enumVal.HasFlag(x)).ToList();
        }
    }
}
