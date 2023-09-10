
using NandToTetris;

// Translates .asm into .hack files
var filePath = @"D:\nand2tetris\projects\06\pong\Pong.asm";

if (!filePath.EndsWith(".asm"))
    throw new ApplicationException("Not a .asm file");

var translatedLines = new List<string>();

try
{
    int LineCounter = 0;

    //first pass
    using StreamReader reader = new StreamReader(filePath);
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(line)) continue;

        line = line.TrimStart();

        if (line.StartsWith("//")) continue;


        if (line.StartsWith('('))
        {
            Parser.AddSymbol(line, LineCounter);
            continue;
        }

        LineCounter++;
    }

    //second pass
    using StreamReader reader1 = new StreamReader(filePath);
    while (!reader1.EndOfStream)
    {
        string? line = reader1.ReadLine();

        if (string.IsNullOrWhiteSpace(line)) continue;

        line = line.TrimStart();
        if (line.StartsWith("//") || line.StartsWith('(')) continue; // comment or symbol

        var parsedLine = Parser.Parse(line);
            
        translatedLines.Add(parsedLine);
    }

    File.WriteAllLines(@"c:\PongTest.hack", translatedLines);
}
catch (FileNotFoundException)
{
    Console.WriteLine("The file does not exist.");
}
catch (IOException ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
