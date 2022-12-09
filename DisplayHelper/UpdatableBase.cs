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

        public virtual bool IsReadOnly { get; set; } = false;

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

        public virtual T Update<T>(T newObject) where T : UpdatableBase
        {
            var properties = newObject.GetType()
                                      .GetProperties()
                                      .Where(x => x.Name != nameof(Id))
                                      .Where(x => x.CanWrite && x.CanRead)
                                      .ToList();

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
                    property.SetValue(this, value);
                }
            }

            return (T)this;
        }

        public static string GetDisplayName()
        {
            return "Row";
        }
    }
}
