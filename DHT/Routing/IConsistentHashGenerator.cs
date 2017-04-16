namespace DHT.Routing
{
    using System;

    public interface IConsistentHashGenerator
    {
        UInt32 Hash(string value);
    }
}
