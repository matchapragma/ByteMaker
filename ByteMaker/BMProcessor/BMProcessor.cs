using System.Security.Cryptography;
using System.Text;

namespace ByteMaker;

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