using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.DisplayHelper
{
    public class ListViewItemsBase<T> where T : UpdatableBase
    {
        private List<T> items = new();

        RadzenDataGrid<T>? itemGrid;
        T? itemToInsert = default;


        public ListViewItemsBase(List<T> items)
        {
            this.items = items;
        }

        public List<T> Items
        {
            get
            {
                return items;
            }
            internal set
            {
                items = value;
            }
        }

        public RadzenDataGrid<T>? ItemGrid { get => itemGrid; set => itemGrid = value; }

        public virtual void OnChangedCollectionChanged()
        {
            
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

            OnChangedCollectionChanged();
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
    }
}
