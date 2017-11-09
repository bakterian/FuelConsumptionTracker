using FCT.Infrastructure.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FCT.Infrastructure.AbstractionClasses
{
    public abstract class RegionCollection : ICollection<RegionBinding>
    {
        protected List<RegionBinding> Regions;

        public RegionCollection()
        {
            Regions = new List<RegionBinding>();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<RegionBinding> GetEnumerator()
        {
            return Regions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RegionBinding item)
        {
            Regions.Add(item);
        }

        public void Clear()
        {
            Regions.Clear();
        }

        public bool Contains(RegionBinding item)
        {
            return Regions.Contains(item);
        }

        public void CopyTo(RegionBinding[] array, int arrayIndex)
        {
            Regions.CopyTo(array, arrayIndex);
        }

        public bool Remove(RegionBinding item)
        {
            return Regions.Remove(item);
        }

        public int Count
        {
            get { return Regions.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(RegionBinding item)
        {
            return Regions.IndexOf(item);
        }

        public void Insert(int index, RegionBinding item)
        {
            Regions.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Regions.RemoveAt(index);
        }

        public RegionBinding this[int index]
        {
            get { return Regions[index]; }
            set { Regions[index] = value; }
        }
    }
}
