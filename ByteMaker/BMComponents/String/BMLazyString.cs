using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ByteMaker
{
        /// <summary>
    /// A string of any length.
    /// </summary>
    public class BMLazyString : BMFileComponent
    {
        private readonly BMTextEncoding encoding;
        private readonly byte[] escapeByte;
        
        /// <param name="fieldName">The name of this item.</param>
        /// <param name="encoding">How to encode the string.</param>
        /// <param name="escapeByte">A byte which represents the end of the string.</param>
        public BMLazyString(string fieldName, BMTextEncoding encoding, byte escapeByte)
        {
            this.fieldName = fieldName;
            this.escapeByte = new [] { escapeByte };
            this.encoding = encoding;
        }
        
        /// <param name="fieldName">The name of this item.</param>
        /// <param name="encoding">How to encode the string.</param>
        /// <param name="escapeByteSequence">A sequence of bytes which represents the
        /// end of the string. This should be no more than bytes characters and used only when the text is Unicode.</param>
        public BMLazyString(string fieldName, BMTextEncoding encoding, byte[] escapeByteSequence)
        {
            this.fieldName = fieldName;
            this.escapeByte = escapeByteSequence;
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
            
            return stringBytes.Concat(escapeByte).ToArray();
        }
        
        public override object Read(ref int index, ref byte[] readBytes)
        {
            int bytesRead = 0;

            List<byte> thisString = new();
            
            if (encoding == BMTextEncoding.Unicode)
            {
                for (int i = index; i < readBytes.Length; i+=2)
                {
                    if (readBytes[i] == escapeByte[0] && readBytes[i + 1] == escapeByte[1]) { bytesRead += 2; break; }
                    thisString.Add(readBytes[i]);
                    thisString.Add(readBytes[i + 1]);
                    bytesRead += 2;
                }
            }
            else
            {
                for (int i = index; i < readBytes.Length; i++)
                {
                    if (readBytes[i] == escapeByte[0]) { bytesRead++; break; }
                    thisString.Add(readBytes[i]);
                    bytesRead ++;
                }
            }
            
            index = index + bytesRead;
            
            return encoding switch
            {
                BMTextEncoding.ASCII => Encoding.ASCII.GetString(thisString.ToArray()),
                #if NET5_0_OR_GREATER
                BMTextEncoding.Latin1 => Encoding.Latin1.GetString(thisString.ToArray()),
                #endif
                BMTextEncoding.Unicode => Encoding.Unicode.GetString(thisString.ToArray()),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}