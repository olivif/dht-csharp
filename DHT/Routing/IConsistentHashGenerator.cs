namespace DHT.Routing
{
    using System;

    public interface IConsistentHashGenerator
    {
        Int32 Hash(string value);
    }
}
