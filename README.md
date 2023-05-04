# ByteMaker
**Simple C# Library for Making Custom Files**

## What?
ByteMaker is a small C# library for writing and reading to files. A single point of reference (`BMFile`) allows you to easily and visually create your file format and easily read or write using this format.

## Installation
Go to Releases and download either the .ZIP or Unity Package. Extract the contents of the .ZIP and drop the files into your C# project or open the Unity Package with your Unity project open.

## How?

### Setup
To work with ByteMaker, you'll need to create a BMFile somewhere in your code. You specify the name of the file, the extension and the structure of the file.

Your file can store:
- Strings of a fixed length `BMFixedString`
- Strings of a variable length `BMLazyString`
- Booleans `BMBoolean`
- All Numeric Types (e.g. sbyte `BMSignedByte`, int `BMInteger`, ulong `BMUnsignedLong`)
- Single and Double Floating Points `BMSingleFloatingPoint`, `BMDoubleFloatingPoint`
- Anything you want! (See "Custom Types" below)

Strings can be encoded/decoded in ASCII, Latin Alphabet No.1 (ISO/IEC 8859-1) and Unicode (Little-Endian)

This is an example file:
```csharp
BMFile file = new("myFile", "locke", new()
{
    new BMFixedString("name", 10, BMTextEncoding.Unicode),
    new BMLazyString("message", BMTextEncoding.ASCII, 0xFF),
});
```

### Writing
With your BMFile ready, you simply call WriteFile on it with the path of the file and a Dictionary of its contents.

```csharp
string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

bool b = file.WriteFile(new Dictionary<string, object>()
{
    {"name", "Annie"},
    {"message", "Good morning starshine, the Earth says hello!!"},
}, path);
```

### Reading
Likewise, call ReadFile on your BMFile. This returns an array of objects which you will need to cast into their correct type.

```csharp
string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

object[] load = file.ReadFile(path);
Console.WriteLine($"Message from {(string)load[0]}: \"{(string)load[1]}\"");
```

The example above will output:
```
Message from Annie: "Good morning starshine, the Earth says hello!!"
```

## Cool Extra Things
### Custom types
You can go beyond the basic types provided by ByteMaker. Create a class which inherits from `BMFileComponent` and write custom writing/reading logic.

Below is an example for writing and reading Vector2s in the context of a Unity game.
```csharp
public class BMVector2: BMFileComponent
{
    public override byte[] Write(object content)
    {
        Vector2 v = (Vector2)content;
        byte[] x = (new BMSingleFloatingPoint("_")).Write(v.x);
        byte[] y = (new BMSingleFloatingPoint("_")).Write(v.y);

        List<byte> bytes = new();
        foreach(byte b in x) { bytes.Add(b); }
        foreach(byte b in y) { bytes.Add(b); }

        return bytes.ToArray();
    }

    public override object Read(ref int index, ref byte[] readBytes)
    {
        byte[] rawV = readBytes[index..(index + 8)];
        byte[] rawX = rawV[0..3];
        byte[] rawY = rawV[4..];

        index += 8;
        return new Vector2(BitConverter.ToSingle(rawX), BitConverter.ToSingle(rawY));
    }

    public BMVector2(string name)
    {
        this.fieldName = name;
    }
}
```

### Processors
Processors are a special object which processes your file's data before it is written. This is useful for including stuff like checksums.
Included in ByteMaker is a simple processor which adds a SHA512 Checksum at the end of your file and will validate it when the file is read.

To use processors, add it to the end of your file definition.
```csharp
BMFile file = new("myFile", "locke", new()
{
    new BMFixedString("name", 10, BMTextEncoding.Unicode),
    new BMLazyString("message", BMTextEncoding.ASCII, 0xFF),
}, new BMEndingChecksumSHA512());
```

## License
ByteMaker is licensed under the Apache 2.0 License. Please refer to the [LICENSE](LICENSE.md) file.
