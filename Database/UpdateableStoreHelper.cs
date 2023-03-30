using RadzenHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.Database
{
    internal class UpdateableStoreHelper
    {
        public static void AddIfNew<T>(UpdateableDataSet<T> updateableDataSet, UpdatableBase newObject) where T : UpdatableBase
        {
            if (updateableDataSet.FullObjects.ContainsKey(newObject.Id))
            {
                return;
            }

            updateableDataSet.FullObjects[newObject.Id] = (UpdatableBase)newObject.Clone();

            List<PropertyInfo> properties = newObject.GetType()
                                                     .GetProperties()
                                                     .Where(x => x.Name != nameof(newObject.Id))
                                                     .Where(x => x.CanWrite && x.CanRead)
                                                     .ToList();

            foreach (var property in properties)
            {
                SetToDefaultOrAddToChildren(updateableDataSet, newObject, property);
            }

            newObject.HasBeenCleared = true;
        }

        private static void SetToDefaultOrAddToChildren<T>(UpdateableDataSet<T> updateableDataSet, UpdatableBase newObject, PropertyInfo property) where T : UpdatableBase
        {
            var value = property.GetValue(newObject);

            if (value is UpdatableBase updateable)
            {
                AddIfNew(updateableDataSet, updateable);
            }
            else if (value is IEnumerable<UpdatableBase> items)
            {
                foreach (var updateableItem in items)
                {
                    AddIfNew(updateableDataSet, updateableItem);
                }
            }
            else if (value?.IsIDictionaryWithValueOfType<UpdatableBase>(out var dict1) == true)
            {
                foreach ((var key, var item) in dict1)
                {
                    AddIfNew(updateableDataSet, (UpdatableBase)item);
                }
            }
            // Cannot be converted to json so don't care...
            //else if (value?.IsIDictionaryWithKeyOfType<UpdatableBase>(out var dict2) == true)
            else
            {
                property.SetValue(newObject, default);
            }
        }
    }
}
