using NandToTetris;

// Translates .vm into .asm files

Console.Write(@"Write the path to the .vm file to be translated. The defaul path is: D:\nand2tetris\projects\06\pong\Pong.asm");
Console.WriteLine("\n");
var arg = Console.ReadLine();
var filePath = !string.IsNullOrWhiteSpace(arg) ? arg : @"D:\nand2tetris\projects\06\pong\Pong.asm";

if (!filePath.EndsWith(".vm"))
    throw new ApplicationException("Not a .vm file");

if (!File.Exists(filePath))
    throw new ApplicationException($"{filePath} does not exist");

var translatedLines = new List<string>();

try
{
    using StreamReader reader = new StreamReader(filePath);
    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(line)) continue;

        line = line.TrimStart();

        var parsedLine = //Parser.Parse(line);

        translatedLines.Add(parsedLine);
    }

    File.WriteAllLines(@$"c:\{DateTime.Now}.asm", translatedLines);
}
catch (FileNotFoundException)
{
    Console.WriteLine("The file does not exist.");
}
catch (IOException ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
