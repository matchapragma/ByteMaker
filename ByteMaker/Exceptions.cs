using System;

namespace ByteMaker
{
    /// <summary>
    /// Exception for when a BMProcessor fails to validate the file.
    /// </summary>
    /// <seealso cref="BMProcessor"/>
    public class FileInvalidException : Exception
    {
        public FileInvalidException()
            : base("A processor failed to validate the file, it may have been tampered with or corrupted.") { }
    }
    
    /// <summary>
    /// Exception for when BMFile tries to read a file that does not exist.
    /// </summary>
    public class PathDoesNotExistException : Exception
    {
        public PathDoesNotExistException(string path)
            : base($"The path \"{path}\" does not exist and therefore the file couldn't be read.") { }
    }
    
    /// <summary>
    /// Exception for when a BMFileComponent that read/writes collections receives too many items to write.
    /// </summary>
    /// <seealso cref="BMFileComponent"/>
    /// <seealso cref="BMFixedIntegerCollection"/>
    public class FixedCollectionComponentException : Exception
    {
        public FixedCollectionComponentException(string componentName, int shouldBe, int was)
            : base($"The FileComponent \"{componentName}\" tried to write a collection that was larger than the " +
                   $"allocated size of {shouldBe}, was {was} items long.") { }
    }
}