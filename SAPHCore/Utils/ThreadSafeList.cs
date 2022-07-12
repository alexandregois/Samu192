using System;
using System.Collections.Generic;
using System.Linq;

namespace SAMU192Core.Utils
{
    internal class ThreadSafeList<T>
    {
        private List<T> _list = new List<T>();
        private object _sync = new object();
        public void Add(T value)
        {
            lock (_sync)
            {
                _list.Add(value);
            }
        }
        public T Find(Predicate<T> predicate)
        {
            lock (_sync)
            {
                return _list.Find(predicate);
            }
        }
        public T FirstOrDefault(Func<T,bool> predicate)
        {
            lock (_sync)
            {
                return _list.FirstOrDefault(predicate);
            }
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            lock (_sync)
            {
                return _list.Where(predicate);
            }
        }

        public int RemoveAll(Predicate<T> predicate)
        {
            lock (_sync)
            {
                return _list.RemoveAll(predicate);
            }
        }

        public void Update(Predicate<T> predicate, T obj)
        {
            lock (_sync)
            {
                _list.RemoveAll(predicate);
                _list.Add(obj);
            }
        }
    }
}
