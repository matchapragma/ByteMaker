using System.Collections.Generic;
using System;

namespace ByteMaker
{
    public class BMSingleFloatingPoint : BMFileComponent
    {
        public override byte[] Write(object content)
        {
            byte[] raw = BitConverter.GetBytes((float)content);
            byte[] actual = new byte[4];
            for (int i = 0; i < raw.Length; i++)
            {
                actual[4 - raw.Length + i] = raw[i];
            }

            return actual;
        }

        public override object Read(ref int index, ref byte[] readBytes)
        { int i = index; index += 4; return BitConverter.ToSingle(readBytes[i..(i + 4)]); }
    
        public BMSingleFloatingPoint(string name)
        {
            this.fieldName = name;
        }
    }

    public class BMDoubleFloatingPoint : BMFileComponent
    {
        public override byte[] Write(object content)
        {
            byte[] raw = BitConverter.GetBytes((float)content);
            byte[] actual = new byte[8];
            for (int i = 0; i < raw.Length; i++)
            {
                actual[8 - raw.Length + i] = raw[i];
            }

            return actual;
        }

        public override object Read(ref int index, ref byte[] readBytes)
        { int i = index; index += 8; return BitConverter.ToDouble(readBytes[i..(i + 8)]); }
    
        public BMDoubleFloatingPoint(string name)
        {
            this.fieldName = name;
        }
    }
}