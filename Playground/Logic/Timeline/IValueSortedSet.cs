namespace Playground.Logic.Timeline
{
    public interface IValueSortedSet
    {
        void CreateEntryType(string key, string name, ulong initialValue = 0);
        void Put(string key, ulong valueToAdd, string name = null);
        void Put(string key, int valueToAdd, string name = null);
        void PostProcess();
    }
}
