using System;
using System.Collections.Generic;
using System.Linq;

namespace c_sharp_playground.Logic.Timeline
{
    public class ValueSortedSet : IValueSortedSet
    {
        private List<ValueSortedSetEntry> DataSet { get; set; }
        private int TotalCount { get; set; }

        public ValueSortedSet()
        {
            DataSet = new List<ValueSortedSetEntry>();
        }

        /// <summary>
        /// Create a data set from the specified parameters
        /// </summary>
        /// <param name="key">Key to reference set by</param>
        /// <param name="name">Readable name of the set</param>
        /// <param name="initialValue">Value to start counting from</param>
        public void CreateEntryType(string key, string name, int initialValue = 0)
        {
            var entry = new ValueSortedSetEntry(key, name, initialValue);
            DataSet.Add(entry);
        }

        /// <summary>
        /// Update the dataset corresponding to the key by adding the value
        /// Creates a new set if no existing set is found for the specified key
        /// </summary>
        /// <param name="key">Key for the data set</param>
        /// <param name="valueToAdd">Value to increment the counter by</param>
        /// <param name="name">Optional name parameter for creation of a new set</param>
        public void Put(string key, int valueToAdd, string name = null)
        {
            var entry = DataSet.FirstOrDefault(entry => entry.Key.Equals(key));
            if (entry == null)
            {
                entry = new ValueSortedSetEntry(key, name ?? key, valueToAdd);
                DataSet.Add(entry);
            }
            TotalCount += valueToAdd;
            entry.Add(valueToAdd);
        }

        /// <summary>
        /// Post processing to update fractions and sort the set
        /// This could be done with each insertion, but this method is somewhat more efficient for many insertions
        /// </summary>
        public void PostProcess()
        {
            DataSet.Sort();
            if (TotalCount != 0)
            {
                DataSet.ForEach(data => data.UpdateFraction(TotalCount));
            }
        }

        /// <summary>
        /// Entry class containing all the information about each counted variation
        /// </summary>
        private class ValueSortedSetEntry : IComparable
        {
            public string Key;
            public string Name;
            private int Count;
            private decimal Fraction;

            public ValueSortedSetEntry(string key, string name, int initialValue = 0)
            {
                Key = key;
                Name = name;
                Count = initialValue;
            }

            public int Add(int value)
            {
                Count += value;
                return Count;
            }

            public int CompareTo(object obj)
            {
                var entry = (ValueSortedSetEntry)obj;
                return Comparer<int>.Default.Compare(Count, entry.Count);
            }

            public int GetCount()
            {
                return Count;
            }

            public decimal UpdateFraction(int totalCount)
            {
                Fraction = (decimal)Count / totalCount;
                return Fraction;
            }
        }
    }
}
