using System.Collections;

namespace ByteMaker
{
    /// <summary>
    /// A collection of booleans (or bits) which has been squashed into bytes.
    /// </summary>
    public class BMSquashedBooleanCollection : BMFileComponent
    {
        /// <summary> The amount of bits/booleans to allocate space to. By default this is 512, which is 64 bytes. </summary>
        private readonly int allocatedSize;
        public int AllocatedSize => allocatedSize;
        public int AllocatedSizeInBytes => allocatedSize / 8;
        
        public override byte[] Write(object content)
        {
            bool[] booleans = (bool[])content;

            BitArray bits = new BitArray(booleans);
        
            byte[] bytes = new byte[AllocatedSizeInBytes];
            bits.CopyTo(bytes, 0);

            return bytes;
        }

        public override object Read(ref int index, ref byte[] readBytes)
        {
            int i = index; index += AllocatedSizeInBytes; var rb = readBytes[i..(i + AllocatedSizeInBytes)];
            
            BitArray bits = new BitArray(rb);
            bool[] bools = new bool[bits.Length];
            bits.CopyTo(bools, 0);

            return bools;
        }

        public BMSquashedBooleanCollection(string fieldName, int allocatedSize = 512)
        {
            this.fieldName = fieldName;
            this.allocatedSize = allocatedSize;
        }
    }
}