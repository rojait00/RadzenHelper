using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper
{
    public enum DisplayMode
    {
        None = 0,
        List = 2,
        Overview = 4,
        Details = 8,
        Edit = 16,
        Always = 9999
    }

    public static class ColumnDisplayModeExtensions
    {
        public static bool ShouldBeVisable(this DisplayMode tableDisplayMode, DisplayMode attributeDisplayMode)
        {
            return tableDisplayMode >= attributeDisplayMode;
         }
    }
}
