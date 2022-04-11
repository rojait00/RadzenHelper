using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper
{
    public class ServiceContainer
    {
        public HttpClient? Http { get; set; }

        public IJSRuntime? JsRuntime { get; set; }

        public NotificationService? NotificationService { get; set; }

        public NavigationManager? NavigationManager { get; set; }
    }
}
