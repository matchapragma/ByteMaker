namespace ByteMaker;

#region Byte

public class BMByte: BMFileComponent
{
    public override byte[] Write(object content) { return new[] { (byte)content }; }

    public override object Read(ref int index, ref byte[] readBytes)
    { index ++; return readBytes[index]; }
}

public class BMSignedByte: BMFileComponent
{
    public override byte[] Write(object content) { return new[] { (byte)(sbyte)content }; }

    public override object Read(ref int index, ref byte[] readBytes)
    { index ++; return (sbyte)readBytes[index]; }
}

#endregion

#region Short

public class BMShort: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((short)content);
        byte[] actual = new byte[2];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[2 - raw.Length - 1 + i] = raw[i];
        }

        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 2; return BitConverter.ToInt32(readBytes[index..(index + 2)]); }
}

public class BMUnsignedShort: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((ushort)content);
        byte[] actual = new byte[2];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[2 - raw.Length - 1 + i] = raw[i];
        }

        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 2; return BitConverter.ToUInt16(readBytes[index..(index + 2)]); }
}

#endregion

#region Integer

public class BMInteger: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((int)content);
        byte[] actual = new byte[4];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[4 - raw.Length - 1 + i] = raw[i];
        }

        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 4; return BitConverter.ToInt32(readBytes[index..(index + 4)]); }
}

public class BMUnsignedInteger: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((uint)content);
        byte[] actual = new byte[4];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[4 - raw.Length - 1 + i] = raw[i];
        }

        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 4; return BitConverter.ToUInt32(readBytes[index..(index + 4)]); }
}

#endregion

#region Long

public class BMLong: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((long)content);
        byte[] actual = new byte[16];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[16 - raw.Length - 1 + i] = raw[i];
        }
        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 16; return BitConverter.ToInt64(readBytes[index..(index + 16)]); }
}

public class BMUnsignedLong: BMFileComponent
{
    public override byte[] Write(object content)
    {
        byte[] raw = BitConverter.GetBytes((ulong)content);
        byte[] actual = new byte[16];
        for (int i = 0; i < raw.Length; i++)
        {
            actual[16 - raw.Length - 1 + i] = raw[i];
        }
        return actual;
    }

    public override object Read(ref int index, ref byte[] readBytes)
    { index += 16; return BitConverter.ToUInt64(readBytes[index..(index + 16)]); }
}

#endregion