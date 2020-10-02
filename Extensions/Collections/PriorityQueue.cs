using System;
using System.Collections.Generic;

namespace Extensions.Collections
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> list;
        public int Count => list.Count;
        private readonly bool IsDescending;

        public PriorityQueue(bool descending = false)
        {
            IsDescending = descending;
            list = new List<T>();
        }

        public PriorityQueue(IEnumerable<T> collection, bool descending = false)
        {
            IsDescending = descending;
            foreach(var item in collection)
                Enqueue(item);
        }
        
        public void Enqueue(T x)
        {
            list.Add(x);
            var i = Count - 1;

            while (i > 0)
            {
                var p = (i - 1) / 2;
                if((IsDescending ? -1 : 1) * list[p].CompareTo(x) <=0) 
                    break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }

        public T Dequeue()
        {
            var target = Peek();
            var root = list[Count - 1];
            list.RemoveAt(Count - 1);

            var i = 0;
            while (i * 2 + 1 < Count)
            {
                var a = i * 2 + 1;
                var b = i * 2 + 2;
                var c = b < Count && (IsDescending ? -1 : 1) * list[b].CompareTo(list[a]) < 0 ? b : a;
                
                if((IsDescending ? -1 : 1) * list[c].CompareTo(root) >= 0)
                    break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
            return target;
        }

        public T Peek()
        {
            if(Count == 0)
                throw new InvalidOperationException("Queue is empty");
            return list[0];
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T key)
        {
            return list.Contains(key);
        }
        
    }
}