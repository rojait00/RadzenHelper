using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper
{
    public class UpdatableBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public UpdatableBase()
        {
        }

        public UpdatableBase(UpdatableBase newValue)
        {
            Update(newValue);
        }

        public static T GetNew<T>() where T : UpdatableBase
        {
            return Activator.CreateInstance<T>();
        }

        public virtual void Update(UpdatableBase newObject)
        {
            var properties = newObject.GetType()
                                      .GetProperties()
                                      .Where(x => x.Name != nameof(Id))
                                      .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(UpdatableBase))
                {
                    var oldValue = (UpdatableBase?)property.GetValue(this) ?? new();
                    var value = (UpdatableBase?)property.GetValue(newObject)  ?? new();
                    oldValue.Update(value);
                }
                else
                {
                    var value = property.GetValue(newObject);
                    property.SetValue(newObject, value);
                }
            }
        }

        public static string GetDisplayName()
        {
            return "Row";
        }
    }
}
