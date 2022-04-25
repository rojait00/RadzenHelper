using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radzen;

namespace RadzenHelper
{
    public class SpecialRowAction<T>
    {
        public SpecialRowAction(string icon, Action<T, ServiceContainer> onClick, ButtonStyle style = ButtonStyle.Primary)
        {
            Icon = icon;
            OnClick = onClick;
            Style = style;
        }

        public Action<T, ServiceContainer> OnClick { get; set; }

        public string Icon { get; set; }

        public ButtonStyle Style { get; set; }
    }
}
