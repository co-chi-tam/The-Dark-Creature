using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPool
{
    public class TDCObjectPool<T> where T : class
    {
        private LinkedList<T> m_ListUsing;
        private Queue<T> m_ListWaiting;

		public T this [int index] {
			get { return m_ListUsing.ElementAt(index); }
			set { 
				var val = m_ListUsing.ElementAt(index);
				val = value;
			}
		}

        public TDCObjectPool()
        {
			m_ListUsing = new LinkedList<T>();
            m_ListWaiting = new Queue<T>();
        }

        public void Create(T item)
        {
            m_ListWaiting.Enqueue(item);
        }

        public T Get()
        {
            T tmp = m_ListWaiting.Dequeue();
            m_ListUsing.AddFirst(tmp);
            return tmp;
        }

        public void Set(T item)
        {
            if (item == null) return;
            T tmp = m_ListUsing.Find(item).Value;
            if (tmp == null) return;
            m_ListUsing.Remove(tmp);
            m_ListWaiting.Enqueue(tmp);
        }

		public void Set(int index)
		{
			T tmp = m_ListUsing.ElementAt (index);
			if (tmp == null) return;
			m_ListUsing.Remove(tmp);
			m_ListWaiting.Enqueue(tmp);
		}

		public int Count() {
			return m_ListUsing.Count;
		}

		public T ElementAtIndex(int index) {
			if (index > m_ListUsing.Count - 1)
				return default (T);
			return m_ListUsing.ElementAt (index);
		}
    }
}
