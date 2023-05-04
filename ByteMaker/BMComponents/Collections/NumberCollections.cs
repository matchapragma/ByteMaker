using System.Collections.Generic;
using System;

namespace ByteMaker
{
    /// <summary>
    /// A fixed-sized collection of Integers.
    /// </summary>
    public class BMFixedIntegerCollection: BMFileComponent
    {
        /// <summary>
        /// The maximum size of the collection.
        /// </summary>
        private readonly int collectionLength;
        
        public BMFixedIntegerCollection(string fieldName, int collectionLength)
        {
            this.fieldName = fieldName;
            this.collectionLength = collectionLength;
        }
        
        public override byte[] Write(object content)
        {
            int[] arr = (int[])content;
            if (arr.Length > collectionLength) { throw new FixedCollectionComponentException(fieldName, collectionLength, arr.Length); }
            List<byte> bytes = new();
            int completed = 0;
            foreach (int i in arr)
            {
                bytes.AddRange(BitConverter.GetBytes((int)i));
                completed++;
            }
            if (completed < collectionLength)
            {
                while (completed < collectionLength)
                {
                    bytes.AddRange(BitConverter.GetBytes(0));
                    completed++;
                }
            }
            return bytes.ToArray();
        }

        public override object Read(ref int index, ref byte[] readBytes)
        {
            byte[] thisCollection = readBytes[index..(index + ( 4 * collectionLength))];

            index += 4 * collectionLength;

            List<int> ints = new();

            for (int i = 0; i < collectionLength; i++)
            {
                ints.Add(BitConverter.ToInt32(thisCollection[(0 + (i * 4))..(4 + (i * 4))]));
            }

            return ints.ToArray();
        }
    }
}