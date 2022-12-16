# ByteMaker
**Simple C# Library for Making Custom Files**

## What?
ByteMaker is a small C# library for writing and reading to files.

## How?

### Setup
To work with ByteMaker, you'll need to create a BMFile somewhere in your code. You specify the name of the file, the extension and the structure of the file.

Your file can store:
- Strings of a fixed length `BMFixedString`
- Strings of a variable length `BMLazyString`
- Booleans `BMBoolean`
- All Numeric Types (e.g. sbyte `BMSignedByte`, int `BMInteger`, ulong `BMUnsignedLong`)

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

## License
ByteMaker is licensed under the Apache 2.0 License. Please refer to the [LICENSE](LICENSE) file.