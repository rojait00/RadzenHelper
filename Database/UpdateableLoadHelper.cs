using RadzenHelper.DisplayHelper;
using RadzenHelper.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RadzenHelper.Database
{
    internal class UpdateableLoadHelper
    {
        internal static UpdatableBase GetFullParent<T>(UpdateableDataSet<T> updateableDataSet) where T : UpdatableBase
        {
            var fullObjectsDict = updateableDataSet.FullObjects;
            var values = fullObjectsDict.Values.OfType<UpdatableBase>().ToList();

            foreach (var fullObject in values)
            {
                var properties = fullObject.GetType()
                                           .GetProperties()
                                           .Where(x => x.Name != nameof(fullObject.Id))
                                           .Where(x => x.CanWrite && x.CanRead)
                                           .ToList();


                foreach (var property in properties)
                {
                    var value = property.GetValue(fullObject);

                    if (value is UpdatableBase updateable)
                    {
                        property.SetValue(fullObject, fullObjectsDict[updateable.Id]);
                    }
                    else if (value is IEnumerable<UpdatableBase> items)
                    {
                        var newItems = items.Select(x => fullObjectsDict[x.Id]).ToList();

                        value = GetValues(value, newItems);

                        property.SetValue(fullObject, value);
                    }
                    else if (value?.IsIDictionaryWithValueOfType<UpdatableBase>(out var dict1) == true)
                    {
                        foreach ((var key, var item) in dict1)
                        {
                            ((IDictionary)value)[key] = fullObjectsDict[item.Id];
                            property.SetValue(fullObject, value);
                        }
                    }
                    // Cannot be converted to json so don't care...
                    //else if (value?.IsIDictionaryWithKeyOfType<UpdatableBase>(out var dict2) == true)
                }
            }

            return fullObjectsDict[updateableDataSet.MinParent.Id];
        }

        private static dynamic GetValues(object value, List<dynamic> newItems)
        {
            if (value.IsListOfType<UpdatableBase>(out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = newItems.ElementAt(i);
                }
                return value;
            }
            if (value.IsCollectionOfType(typeof(Array)))
            {
                for (int i = 0; i < ((dynamic[])value).Length; i++)
                {
                    ((dynamic[])value)[i] = newItems.ElementAt(i);
                }
                return value;
            }
            //if (value.IsCollectionOfType(typeof(IEnumerable<>)))
            //{
            //    ((IEnumerable<dynamic>)value).?????; // Cannot modify => no support
            //    return newItems;
            //}

            throw new Exception("unknown list type: " + value.GetType().Name);
        }
    }
}
