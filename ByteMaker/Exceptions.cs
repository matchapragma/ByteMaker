using System;

namespace ByteMaker
{
    public class FileInvalidException : Exception
    {
        public FileInvalidException()
            : base("A processor failed to validate the file, it may have been tampered with or corrupted.") { }
    }
    
    public class PathDoesNotExistException : Exception
    {
        public PathDoesNotExistException(string path)
            : base($"The path \"{path}\" does not exist and therefore the file couldn't be read.") { }
    }
}