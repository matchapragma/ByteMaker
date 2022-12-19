namespace ByteMaker
{
    /// <summary>
    /// A boolean represented by a single byte.
    /// </summary>
    public class BMBoolean: BMFileComponent
    {
        private readonly byte onRepresentation;
        private readonly byte offRepresentation;
        
        public override byte[] Write(object content)
        {
            bool b = (bool)content;
            return new[] { b ? onRepresentation : offRepresentation };
        }

        public override object Read(ref int index, ref byte[] readBytes)
        {
            int i = index;
            index++;
            return readBytes[i] == onRepresentation;
        }

        public BMBoolean(string name, byte on = 0xFF, byte off = 0x00)
        {
            this.fieldName = name;
            this.onRepresentation = on;
            this.offRepresentation = off;
        }
    }
}