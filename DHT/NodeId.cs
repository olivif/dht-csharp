namespace DHT
{
    using System;

    public class NodeId
    {
        private readonly int numberOfBytes = 20;

        private readonly byte[] rawBytes;

        public byte[] RawBytes
        {
            get { return this.rawBytes; }
        }

        /// <summary>
        /// Construct a randomly generated node id 
        /// </summary>
        public NodeId()
        {
            this.rawBytes = this.GetRandomBytes(this.numberOfBytes);
        }

        /// <summary>
        /// Calculate the distance between two nodes
        /// </summary>
        public int GetDistance(NodeId other)
        {
            var distance = 0;
            for (int byteIndex = 0; byteIndex < other.RawBytes.Length; byteIndex++)
            {
                distance += this.rawBytes[byteIndex] ^ other.RawBytes[byteIndex];
            }

            return distance;
        }

        /// <summary>
        /// Convert the node id to a string
        /// </summary>
        public override string ToString()
        {
            string hex = BitConverter.ToString(this.rawBytes);
            return hex.Replace("-", string.Empty);
        }

        /// <summary>
        /// Generate random byte array
        /// </summary>
        private byte[] GetRandomBytes(int numberOfBytes)
        {
            byte[] bytes = new byte[numberOfBytes];

            var randomSeed = Guid.NewGuid().GetHashCode();
            var random = new Random();
            random.NextBytes(bytes);

            return bytes;
        }
    }
}
