using Radzen.Blazor;
using RadzenHelper.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper
{
    public class ListViewItemsBase<T> where T : UpdatableBase
    {
        private List<T> items = new();

        RadzenDataGrid<T>? itemGrid;
        T? itemToInsert = default;


        public ListViewItemsBase()
        {

        }
        public virtual async Task Init()
        {
            await LoadItems();
        }


        public List<T> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        public T? ItemToInsert { get => itemToInsert; set => itemToInsert = value; }

        public RadzenDataGrid<T>? ItemGrid { get => itemGrid; set => itemGrid = value; }

        public virtual async Task OnChangedCollectionChanged(T? item)
        {
            if (itemGrid != null)
            {
                await itemGrid.Reload();
            }
        }



        public virtual List<(PropertyInfo Info, ColumnDefinitionAttribute Attribute)> GetColumns()
        {
            return Items.GetNestedType()
                        .GetPropertyWithAttribute<ColumnDefinitionAttribute>()
                        ?? new();
        }

        public virtual async Task InsertRow()
        {
            if (itemGrid == default)
                return;

            itemToInsert = UpdatableBase.GetNew<T>();
            await itemGrid.InsertRow(itemToInsert);
        }


        public virtual async Task LoadItems()
        {
            if (itemGrid != null)
            {
                await itemGrid.Reload();
            }
        }

        public virtual async Task OnCreateRow(T item)
        {
            await AddOrUpdateAsync(item);
        }

        public virtual async Task EditRow(T item)
        {
            if (itemGrid == default)
                return;

            await itemGrid.EditRow(item);
        }

        public virtual async Task OnUpdateRow(T item)
        {
            if (item == itemToInsert)
            {
                itemToInsert = default;
            }

            await AddOrUpdateAsync(item);
        }

        public virtual async Task SaveRow(T item)
        {
            if (itemGrid == default)
                return;

            if (item == itemToInsert)
            {
                itemToInsert = default;
            }

            await itemGrid.UpdateRow(item);

            await OnChangedCollectionChanged(item);
        }

        public virtual void CancelEdit(T item)
        {
            if (itemGrid == default)
                return;

            if (item == itemToInsert)
            {
                itemToInsert = default;
            }

            itemGrid.CancelEditRow(item);
        }

        public virtual async Task DeleteRow(T item)
        {
            if (itemGrid == default)
                return;

            if (item == itemToInsert)
            {
                itemToInsert = default;
            }

            if (items.Contains(item))
            {
                items.Remove(item);

                await itemGrid.Reload();
            }
            else
            {
                itemGrid.CancelEditRow(item);
            }
        }

        private async Task AddOrUpdateAsync(T item)
        {
            if (itemGrid == default)
                return;

            var tempRule = items.FirstOrDefault(x => x.Id == item.Id);
            if (tempRule != null)
            {
                tempRule.Update(item);
                await itemGrid.UpdateRow(item);
            }
            else
            {
                items.Add(item);
                await itemGrid.UpdateRow(item);
            }
            await itemGrid.Reload();
        }

        public string GetDisplayName()
        {
            var methodeName = nameof(UpdatableBase.GetDisplayName);
            return (string?)typeof(T).GetMethod(methodeName)?.Invoke(null, null) ?? UpdatableBase.GetDisplayName();
        }
    }
}
