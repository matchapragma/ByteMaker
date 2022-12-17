namespace ByteMaker;

/// <summary>
/// A boolean represented by a single byte.
/// </summary>
public class BMBoolean: BMFileComponent
{
    private readonly byte onRepresentation;
    private readonly byte offRepresentation;

    public BMBoolean(byte onRepresentation = 0xFF, byte offRepresentation = 0x00)
    {
        this.onRepresentation = onRepresentation;
        this.offRepresentation = offRepresentation;
    }
    
    public override byte[] Write(object content)
    {
        bool b = (bool)content;
        return new[] { b ? onRepresentation : offRepresentation };
    }

    public override object Read(ref int index, ref byte[] readBytes)
    {
        return readBytes[index] == onRepresentation;
        index++;
    }
}