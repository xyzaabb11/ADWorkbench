using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AD.Workbench.Serivces
{
    static class ADExtensions
    {
        #region Service Provider Extensions
        /// <summary>
        /// Retrieves the service of type <c>T</c> from the provider.
        /// If the service cannot be found, this method returns <c>null</c>.
        /// </summary>
        public static T GetService<T>(this IServiceProvider provider) where T : class
        {
            return (T)provider.GetService(typeof(T));
        }

        /// <summary>
        /// Retrieves the service of type <c>T</c> from the provider.
        /// If the service cannot be found, a <see cref="ServiceNotFoundException"/> will be thrown.
        /// </summary>
        public static T GetRequiredService<T>(this IServiceProvider provider) where T : class
        {
            return (T)GetRequiredService(provider, typeof(T));
        }

        /// <summary>
        /// Retrieves the service of type <paramref name="serviceType"/> from the provider.
        /// If the service cannot be found, a <see cref="ServiceNotFoundException"/> will be thrown.
        /// </summary>
        public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
        {
            object service = provider.GetService(serviceType);
            if (service == null)
                throw new ServiceNotFoundException(serviceType);
            return service;
        }
        #endregion

        #region DPI independence
        public static Rect TransformToDevice(this Rect rect, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return Rect.Transform(rect, matrix);
        }

        public static Rect TransformFromDevice(this Rect rect, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return Rect.Transform(rect, matrix);
        }

        public static Size TransformToDevice(this Size size, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new Size(size.Width * matrix.M11, size.Height * matrix.M22);
        }

        public static Size TransformFromDevice(this Size size, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return new Size(size.Width * matrix.M11, size.Height * matrix.M22);
        }

        public static Point TransformToDevice(this Point point, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return matrix.Transform(point);
        }

        public static Point TransformFromDevice(this Point point, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return matrix.Transform(point);
        }
        #endregion

        #region Collections
        /// <summary>
        /// Obsolete. Please use a regular foreach loop instead. ForEach() is executed for its side-effects, and side-effects mix poorly with a functional programming style.
        /// </summary>
        //[Obsolete("Please use a regular foreach loop instead. ForEach() is executed for its side-effects, and side-effects mix poorly with a functional programming style.")]
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            foreach (T element in input)
            {
                action(element);
            }
        }

        /// <summary>
        /// Adds all <paramref name="elements"/> to <paramref name="list"/>.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> elements)
        {
            foreach (T o in elements)
                list.Add(o);
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> arr)
        {
            return new ReadOnlyCollection<T>(arr);
        }

//         [Obsolete("This method seems to be unused now; all uses I've seen have been replaced with IReadOnlyList<T>")]
//         public static ReadOnlyCollectionWrapper<T> AsReadOnly<T>(this ICollection<T> arr)
//         {
//             return new ReadOnlyCollectionWrapper<T>(arr);
//         }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> dict, K key)
        {
            V ret;
            dict.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// Searches a sorted list
        /// </summary>
        /// <param name="list">The list to search in</param>
        /// <param name="key">The key to search for</param>
        /// <param name="keySelector">Function that maps list items to their sort key</param>
        /// <param name="keyComparer">Comparer used for the sort</param>
        /// <returns>Returns the index of the element with the specified key.
        /// If no such element is found, this method returns a negative number that is the bitwise complement of the
        /// index where the element could be inserted while maintaining the order.</returns>
        public static int BinarySearch<T, K>(this IList<T> list, K key, Func<T, K> keySelector, IComparer<K> keyComparer = null)
        {
            return BinarySearch(list, 0, list.Count, key, keySelector, keyComparer);
        }

        /// <summary>
        /// Searches a sorted list
        /// </summary>
        /// <param name="list">The list to search in</param>
        /// <param name="index">Starting index of the range to search</param>
        /// <param name="length">Length of the range to search</param>
        /// <param name="key">The key to search for</param>
        /// <param name="keySelector">Function that maps list items to their sort key</param>
        /// <param name="keyComparer">Comparer used for the sort</param>
        /// <returns>Returns the index of the element with the specified key.
        /// If no such element is found in the specified range, this method returns a negative number that is the bitwise complement of the
        /// index where the element could be inserted while maintaining the order.</returns>
        public static int BinarySearch<T, K>(this IList<T> list, int index, int length, K key, Func<T, K> keySelector, IComparer<K> keyComparer = null)
        {
            if (keyComparer == null)
                keyComparer = Comparer<K>.Default;
            int low = index;
            int high = index + length - 1;
            while (low <= high)
            {
                int mid = low + (high - low >> 1);
                int r = keyComparer.Compare(keySelector(list[mid]), key);
                if (r == 0)
                {
                    return mid;
                }
                else if (r < 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }
            return ~low;
        }

        /// <summary>
        /// Inserts an item into a sorted list.
        /// </summary>
        public static void OrderedInsert<T>(this IList<T> list, T item, IComparer<T> comparer)
        {
            int pos = BinarySearch(list, item, x => x, comparer);
            if (pos < 0)
                pos = ~pos;
            list.Insert(pos, item);
        }

        /// <summary>
        /// Sorts the enumerable using the given comparer.
        /// </summary>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> input, IComparer<T> comparer)
        {
            return Enumerable.OrderBy(input, e => e, comparer);
        }

        /// <summary>
        /// Converts a recursive data structure into a flat list.
        /// </summary>
        /// <param name="input">The root elements of the recursive data structure.</param>
        /// <param name="recursion">The function that gets the children of an element.</param>
        /// <returns>Iterator that enumerates the tree structure in preorder.</returns>
//         public static IEnumerable<T> Flatten<T>(this IEnumerable<T> input, Func<T, IEnumerable<T>> recursion)
//         {
//             return ICSharpCode.NRefactory.Utils.TreeTraversal.PreOrder(input, recursion);
//         }


        /// <summary>
        /// Creates an array containing a part of the array (similar to string.Substring).
        /// </summary>
        public static T[] Splice<T>(this T[] array, int startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            return Splice(array, startIndex, array.Length - startIndex);
        }

        /// <summary>
        /// Creates an array containing a part of the array (similar to string.Substring).
        /// </summary>
        public static T[] Splice<T>(this T[] array, int startIndex, int length)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (startIndex < 0 || startIndex > array.Length)
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "Value must be between 0 and " + array.Length);
            if (length < 0 || length > array.Length - startIndex)
                throw new ArgumentOutOfRangeException("length", length, "Value must be between 0 and " + (array.Length - startIndex));
            T[] result = new T[length];
            Array.Copy(array, startIndex, result, 0, length);
            return result;
        }

        public static IEnumerable<T> DistinctBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector) where K : IEquatable<K>
        {
            // Don't just use .Distinct(KeyComparer.Create(keySelector)) - that would evaluate the keySelector multiple times.
            var hashSet = new HashSet<K>();
            foreach (var element in source)
            {
                if (hashSet.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Returns the minimum element.
        /// </summary>
        /// <exception cref="InvalidOperationException">The input sequence is empty</exception>
        public static T MinBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector) where K : IComparable<K>
        {
            return source.MinBy(keySelector, Comparer<K>.Default);
        }

        /// <summary>
        /// Returns the minimum element.
        /// </summary>
        /// <exception cref="InvalidOperationException">The input sequence is empty</exception>
        public static T MinBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> keyComparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("selector");
            if (keyComparer == null)
                keyComparer = Comparer<K>.Default;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");
                T minElement = enumerator.Current;
                K minKey = keySelector(minElement);
                while (enumerator.MoveNext())
                {
                    T element = enumerator.Current;
                    K key = keySelector(element);
                    if (keyComparer.Compare(key, minKey) < 0)
                    {
                        minElement = element;
                        minKey = key;
                    }
                }
                return minElement;
            }
        }

        /// <summary>
        /// Returns the maximum element.
        /// </summary>
        /// <exception cref="InvalidOperationException">The input sequence is empty</exception>
        public static T MaxBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector) where K : IComparable<K>
        {
            return source.MaxBy(keySelector, Comparer<K>.Default);
        }

        /// <summary>
        /// Returns the maximum element.
        /// </summary>
        /// <exception cref="InvalidOperationException">The input sequence is empty</exception>
        public static T MaxBy<T, K>(this IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> keyComparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("selector");
            if (keyComparer == null)
                keyComparer = Comparer<K>.Default;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");
                T maxElement = enumerator.Current;
                K maxKey = keySelector(maxElement);
                while (enumerator.MoveNext())
                {
                    T element = enumerator.Current;
                    K key = keySelector(element);
                    if (keyComparer.Compare(key, maxKey) > 0)
                    {
                        maxElement = element;
                        maxKey = key;
                    }
                }
                return maxElement;
            }
        }

        /// <summary>
        /// Returns the index of the first element for which <paramref name="predicate"/> returns true.
        /// If none of the items in the list fits the <paramref name="predicate"/>, -1 is returned.
        /// </summary>
        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first element for which <paramref name="predicate"/> returns true.
        /// If none of the items in the list fits the <paramref name="predicate"/>, -1 is returned.
        /// </summary>
 /*       public static int FindIndex<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }

            return -1;
        }
        */
        /// <summary>
        /// Adds item to the list if the item is not null.
        /// </summary>
        public static void AddIfNotNull<T>(this IList<T> list, T itemToAdd) where T : class
        {
            if (itemToAdd != null)
                list.Add(itemToAdd);
        }

        public static void RemoveAll<T>(this IList<T> list, Predicate<T> condition)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            int i = 0;
            while (i < list.Count)
            {
                if (condition(list[i]))
                    list.RemoveAt(i);
                else
                    i++;
            }
        }
        #endregion


    }
}
