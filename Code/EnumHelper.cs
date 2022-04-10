using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.Code
{
    public static class EnumHelper
    {
        public static List<Enum> GetEnums<T>(this T enumVal) where T : Enum
        {
            var values = Enum.GetValues(enumVal.GetType());
            return values.Cast<Enum>().ToList();
        }
    }
}
