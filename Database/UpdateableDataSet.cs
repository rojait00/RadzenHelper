
using Newtonsoft.Json;

namespace RadzenHelper.Database
{
    public class UpdateableDataSet<T> where T : UpdatableBase
    {
        public T MinParent { get; set; } = UpdatableBase.GetNew<T>();

        [JsonIgnore]
        public T FullParent
        {
            get
            {
                return (T)UpdateableLoadHelper.GetFullParent<T>(this);
            }

            set
            {
                FullObjects.Clear();
                FullObjects.Add(value.Id, value);
                
                var newValue = (UpdatableBase)value.Clone();
                UpdateableStoreHelper.AddIfNew(this, newValue);
                MinParent = (T)newValue;
            }
        }

        public Dictionary<Guid, dynamic> FullObjects { get; set; } = new();

    }
}
