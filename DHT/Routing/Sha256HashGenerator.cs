﻿namespace DHT.Routing
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Sha256HashGenerator : IConsistentHashGenerator
    {
        private readonly SHA256 hasher;

        public Sha256HashGenerator()
        {
            this.hasher = SHA256.Create();
        }

        public UInt32 Hash(string value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException();

            using (var stream = this.GenerateStreamFromString(value))
            {
                var hashBytes = this.hasher.ComputeHash(stream);
                var hashInt = BitConverter.ToUInt32(hashBytes, 0);

                return hashInt;
            }
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}
