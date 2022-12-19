using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ByteMaker
{
    /// <summary>
    /// Creates a SHA512 checksum and appends it to the end of the file. Validates the checksum when the file is read.
    /// </summary>
    public class BMEndingChecksumSHA512 : BMProcessor
    {
        public override void Process(ref List<byte[]> fileInMaking)
        {
            List<byte> byteArray = new();
            foreach (byte[] arr in fileInMaking)
            {
                foreach(byte b in arr) {  byteArray.Add(b); }
            }

            SHA512 sha512 = new SHA512Managed();
            byte[] hash = sha512.ComputeHash(byteArray.ToArray());
            fileInMaking.Add(hash);
        }

        public override void Strip(ref byte[] file)
        {
            Array.Resize(ref file, file.Length - 64);
        }

        public override bool Query(ref byte[] file)
        {
            SHA512 sha512 = new SHA512Managed();
            byte[] fileStrippedOfHash = file[0..^64];
            byte[] fileHash = file[^64..];
            byte[] hash = sha512.ComputeHash(fileStrippedOfHash);

            return fileHash.SequenceEqual(hash);
        }
    }
}