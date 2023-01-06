using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.DisplayHelper
{
    public class AppointmentTypedData<T> : AppointmentData
    {
        public T Value
        {
            get
            {
                return (T)Data; //(Data.GetType() == typeof(T) ? (T)Data : throw new Exception("Don't fill AppointmentData.Data directly!"));
            }

            set => Data = value;
        }
    }
}
