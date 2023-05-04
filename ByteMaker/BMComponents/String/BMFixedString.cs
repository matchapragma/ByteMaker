using System;
using System.Collections.Generic;
using System.Text;

namespace ByteMaker
{
    /// <summary>
    /// A string with a fixed length.
    /// </summary>
    public class BMFixedString : BMFileComponent
    {
        private readonly int stringLength;
        private readonly BMTextEncoding encoding;
        
        /// <param name="fieldName">The name of this item.</param>
        /// <param name="stringLength">The amount of bytes allocated for this string.</param>
        /// <param name="encoding">How to encode the string.</param>
        public BMFixedString(string fieldName, int stringLength, BMTextEncoding encoding)
        {
            this.fieldName = fieldName;
            this.stringLength = stringLength;
            this.encoding = encoding;
        }

        public override byte[] Write(object content)
        {
            string contents = (string)content;

            byte[] stringBytes = encoding switch
            {
                BMTextEncoding.ASCII => Encoding.ASCII.GetBytes(contents),
                #if NET5_0_OR_GREATER
                BMTextEncoding.Latin1 => Encoding.Latin1.GetBytes(contents),
                #endif
                BMTextEncoding.Unicode => Encoding.Unicode.GetBytes(contents),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            int factor = encoding == BMTextEncoding.Unicode ? 2 : 1;
            byte[] arr = new byte[stringLength * factor];
            for (int i = 0; i < stringBytes.Length; i++)
            {
                byte s = stringBytes[i];
                arr[i] = s;
            }
            return arr;
        }
        
        public override object Read(ref int index, ref byte[] readBytes)
        {
            int len = stringLength * (encoding == BMTextEncoding.Unicode ? 2 : 1);
            byte[] thisString = readBytes[index..(index + len)];
            
            index = index + len;
            
            return encoding switch
            {
                BMTextEncoding.ASCII => Encoding.ASCII.GetString(thisString),  
                #if NET5_0_OR_GREATER
                BMTextEncoding.Latin1 => Encoding.Latin1.GetString(thisString),
                #endif
                BMTextEncoding.Unicode => Encoding.Unicode.GetString(thisString),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}