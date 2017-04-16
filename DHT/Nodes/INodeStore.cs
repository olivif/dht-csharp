namespace DHT.Nodes
{
    /// <summary>
    /// Interface for node storage
    /// </summary>
    public interface INodeStore
    {
        /// <summary>
        /// Does this store contain this key
        /// </summary>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets a value (if there). Null if not.
        /// </summary>
        string GetValue(string key);

        /// <summary>
        /// Adds a key, value
        /// </summary>
        bool AddValue(string key, string value);

        /// <summary>
        /// Removes a key, value
        /// </summary>
        bool RemoveValue(string key);
    }
}
