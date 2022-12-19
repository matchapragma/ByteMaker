using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ByteMaker
{
    /// <summary>
    /// Processes your file before it is written.
    /// </summary>
    public abstract class BMProcessor
    {
        /// <summary>
        /// The process to apply after the file has been created, but not written to.
        /// </summary>
        public abstract void Process(ref List<byte[]> fileInMaking);

        /// <summary>
        /// Strips the changes made by this processor.
        /// </summary>
        public abstract void Strip(ref byte[] file);

        /// <summary>
        /// Allows your processor to validate your file. Use this for checksums and etcetera. Otherwise, simply return true.
        /// </summary>
        /// <returns>Whether this processor is satisfied with the read file.</returns>
        public abstract bool Query(ref byte[] file);
    }

}
