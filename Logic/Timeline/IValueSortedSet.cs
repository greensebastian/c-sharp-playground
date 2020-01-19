namespace c_sharp_playground.Logic.Timeline
{
    public interface IValueSortedSet
    {
        void CreateEntryType(string key, string name, int initialValue = 0);
        void Put(string key, int valueToAdd, string name = null);
        void PostProcess();
    }
}
