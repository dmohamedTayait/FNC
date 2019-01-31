using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    // ----------------------------------------------------------------------------------
    // Some necessary classes & interfaces

    interface IAdjacentGrouping<T> : IEnumerable<T> { }

    class WrappedAdjacentGrouping<T> : IAdjacentGrouping<T>
    {
        public IEnumerable<T> Wrapped { get; set; }
        public IEnumerator<T> GetEnumerator()
        {
            return Wrapped.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    class Grouping<K, T> : IGrouping<K, T>
    {
        public K Key { get; set; }
        public IEnumerable<T> Elements;

        public IEnumerator<T> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    // ----------------------------------------------------------------------------------
    // Extnesion methods for converting type and custom GroupBy implementation

    static class MovingExtensions
    {
        public static IAdjacentGrouping<T> WithAdjacentGrouping<T>(this IEnumerable<T> e)
        {
            return new WrappedAdjacentGrouping<T> { Wrapped = e };
        }

        public static IEnumerable<IGrouping<K, T>> GroupBy<T, K>(this IAdjacentGrouping<T> source,
             Func<T, K> keySelector) where K : IComparable
        {
            // Remembers elements of the current group
            List<T> elementsSoFar = new List<T>();
            IEnumerator<T> en = source.GetEnumerator();
            if (en.MoveNext())
            {
                K lastKey = keySelector(en.Current);
                do
                {
                    // Test whether current element starts a new group
                    K newKey = keySelector(en.Current);
                    if (newKey.CompareTo(lastKey) != 0)
                    {
                        // Yield the previous group and start next one
                        yield return new Grouping<K, T> { Elements = elementsSoFar, Key = lastKey };
                        lastKey = newKey;
                        elementsSoFar = new List<T>();
                    }
                    // Add element to the current group
                    elementsSoFar.Add(en.Current);
                } while (en.MoveNext());
                // Yield the last group of sequence
                yield return new Grouping<K, T> { Elements = elementsSoFar, Key = lastKey };
            }
        }
    }



}


