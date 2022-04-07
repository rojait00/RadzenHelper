using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnDefinitionAttribute : Attribute
    {

        public ColumnDefinitionAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        /// <summary>
        /// default: false;
        /// </summary>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        /// default: false;
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// default: Always
        /// </summary>
        public DisplayMode DisplayMode { get; set; } = DisplayMode.Always;
    }
}