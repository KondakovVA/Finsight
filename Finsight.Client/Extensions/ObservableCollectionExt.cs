using System.Collections.ObjectModel;

namespace Finsight.Client.Extensions
{
    public static class ObservableCollectionExt
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> addingCollection)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(addingCollection);

            foreach (var item in addingCollection)
            {
                collection.Add(item);
            }
        }

        public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(newItems);

            collection.Clear();
            collection.AddRange(newItems);
        }
    }
}
