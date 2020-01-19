using System;
using System.Collections.Generic;
using System.Linq;

namespace c_sharp_playground.Logic.Timeline
{
    public class ValueSortedSet : IValueSortedSet
    {
        public List<ValueSortedSetEntry> DataSet { get; private set; }
        public ulong TotalCount { get; private set; }

        public string Unit { get; private set; }

        public ValueSortedSet(string unit)
        {
            Unit = unit;
            DataSet = new List<ValueSortedSetEntry>();
        }

        /// <summary>
        /// Create a data set from the specified parameters
        /// </summary>
        /// <param name="key">Key to reference set by</param>
        /// <param name="name">Readable name of the set</param>
        /// <param name="initialValue">Value to start counting from</param>
        public void CreateEntryType(string key, string name, ulong initialValue = 0)
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
        public void Put(string key, ulong valueToAdd, string name = null)
        {
            var entry = DataSet.FirstOrDefault(entry => entry.Key.Equals(key));
            if (entry == null)
            {
                entry = new ValueSortedSetEntry(key, name ?? key);
                DataSet.Add(entry);
            }
            TotalCount += valueToAdd;
            entry.Add(valueToAdd);
        }

        public void Put(string key, int valueToAdd, string name = null)
        {
            Put(key, (ulong)valueToAdd, name);
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
                var highestFraction = DataSet.First().Fraction;
                DataSet.ForEach(data => data.UpdateNormalizedFraction(highestFraction));
            }
        }

        /// <summary>
        /// Entry class containing all the information about each counted variation
        /// </summary>
        public class ValueSortedSetEntry : IComparable
        {
            public string Key { get; private set; }
            public string Name { get; private set; }
            public ulong Count { get; private set; }
            public decimal Fraction { get; private set; }
            public decimal NormalizedFraction { get; private set; }

            public ValueSortedSetEntry(string key, string name, ulong initialValue = 0)
            {
                Key = key;
                Name = name;
                Count = initialValue;
            }

            public ulong Add(ulong value)
            {
                Count += value;
                return Count;
            }

            public int CompareTo(object obj)
            {
                var entry = (ValueSortedSetEntry)obj;
                // Inverted order to get descending sort
                return Comparer<ulong>.Default.Compare(entry.Count, Count);
            }

            public decimal UpdateFraction(ulong totalCount)
            {
                Fraction = (decimal)Count / totalCount;
                return Fraction;
            }

            public decimal UpdateNormalizedFraction(decimal topFraction)
            {
                NormalizedFraction = Fraction / topFraction;
                return NormalizedFraction;
            }
        }
    }
}
