namespace FruitMatch.Scripts.System
{
    public class HashCodeHelper
    {
        public int hash;

        public HashCodeHelper() => hash = GetHash();

        public int GetHash() => GetHashCode();
    }
}