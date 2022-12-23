using System.Collections.Generic;
using System.IO;

// ByteMaker A1.0
// (C) LOCKE 2022, Written by Annie Locke

namespace ByteMaker
{
    /// <summary>
    /// A ByteMaker file.
    /// </summary>
    public class BMFile
    {
        public string fileName;
        public string extension;

        private List<BMFileComponent> components = new();
        private BMProcessor processor;
        
        /// <summary>
        /// Reads a file and creates an array of objects per the BMFile's configuration.
        /// </summary>
        /// <param name="path">The path of the file to read.</param>
        /// <param name="fileName">Override the name of the file that was defined by the BMFile.</param>
        /// <param name="extension">Override the extension of the file that was defined by the BMFile.</param>
        /// <returns>An array of objects. Returns null if failed.</returns>
        public object[] ReadFile(string path, string? fileName = null, string? extension = null)
        {
            string filePath = $"{path}/{fileName ?? this.fileName}.{extension ?? this.extension}";

            if (File.Exists(filePath)) { throw new PathDoesNotExistException(filePath); }
            
            try
            {
                byte[] readBytes = File.ReadAllBytes(filePath);

                if (processor != null)
                {
                    if (!processor.Query(ref readBytes))
                    {
                        return null;
                    }
                    processor.Strip(ref readBytes);
                }

                int byteIndex = 0;

                List<object> objects = new();

                foreach (BMFileComponent comp in components)
                {
                    if (byteIndex >= readBytes.Length)
                    {
                        break;
                    }
                    
                    objects.Add(comp.Read(ref byteIndex, ref readBytes));
                }
                
                return objects.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Writes a file per the BMFile's configuration.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="path">The path of the file.</param>
        /// <param name="fileName">Override the name of the file that was defined by the BMFile.</param>
        /// <param name="extension">Override the extension of the file that was defined by the BMFile.</param>
        /// <returns>Returns true if the file was written to successfully, otherwise false.</returns>
        public bool WriteFile(Dictionary<string, object> contents, string path, string? fileName = null, string? extension = null)
        {
            List<byte[]> bytes = new();
            
            foreach (BMFileComponent comp in components)
            {
                bytes.Add(comp.Write(contents[comp.FieldName]));
            }

            if (extension != null)
            {
                if (extension.ToCharArray()[0] == '.')
                {
                    extension = extension.TrimStart('.');
                }
            }
            
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            string filePath = $"{path}/{fileName ?? this.fileName}.{extension ?? this.extension}";

            if (processor != null) { processor.Process(ref bytes); }
            
            List<byte> byteArray = new();
            foreach (byte[] arr in bytes)
            {
                foreach(byte b in arr) {  byteArray.Add(b); }
            }

            try
            {
                File.WriteAllBytes(filePath, byteArray.ToArray());
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Create a new ByteMaker file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="extension">The extension of the file sans starting full stop.</param>
        /// <param name="components">The fields of this file.</param>
        /// <param name="processor">Processes the file before it is written.</param>
        public BMFile(string fileName, string extension, List<BMFileComponent> components, BMProcessor? processor = null)
        {
            this.fileName = fileName;
            if (extension.ToCharArray()[0] == '.')
            {
                this.extension = extension.TrimStart('.');
            }
            else
            {
                this.extension = extension;
            }
            this.components = components;
            this.processor = processor;
        }
    }
}