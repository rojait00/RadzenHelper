using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radzen;

namespace RadzenHelper
{
    public class SpecialRowAction<T>
    {
        public SpecialRowAction(string icon, Func<T, ServiceContainer,Task> onClick, ButtonStyle style = ButtonStyle.Primary)
        {
            Icon = icon;
            OnClick = onClick;
            Style = style;
        }

        public Func<T, ServiceContainer, Task> OnClick { get; set; }

        public string Icon { get; set; }

        public ButtonStyle Style { get; set; }
    }
}
