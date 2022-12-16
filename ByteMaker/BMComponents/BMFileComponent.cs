namespace ByteMaker
{
    /// <summary>
    /// A BMFileComponent represents any section of data that is written to a BMFile.
    /// </summary>
    public abstract class BMFileComponent
    {
        public abstract byte[] Write(object content);
        public abstract object Read(ref int index, ref byte[] readBytes);
        
        protected string fieldName;
        public string FieldName => fieldName;
    }
}