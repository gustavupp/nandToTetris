
namespace NandToTetris
{
    public class Parser
    {
        private static readonly Dictionary<string, string> _comp =  new Dictionary<string, string>
        {
            { "0" , "0101010" },
            { "1" , "0111111" }, 
            { "-1", "0111010" }, 
            { "D", "0001100" }, 
            { "A", "0110000" }, 
            { "!D", "0001101" }, 
            { "!A", "0110001" },
            { "-D", "0001111" },
            { "-A", "0110011" },
            { "D+1", "0011111" },
            { "A+1", "0110111" },
            { "D-1", "0001110" },
            { "A-1", "0110010" },
            { "D+A", "0000010" },
            { "D-A", "0010011" },
            { "A-D", "0000111" },
            { "D&A", "0000000" },
            { "D|A", "0010101" },
            { "M", "1110000" },
            { "!M", "1110001" },
            { "-M", "1110011" },
            { "M+1", "1110111" },
            { "M-1", "1110010" },
            { "D+M", "1000010" },
            { "D-M", "1010011" },
            { "M-D", "1000111" },
            { "D&M", "1000000" },
            { "D|M", "1010101" },
        };
        private static readonly Dictionary<string, string> _dest = new Dictionary<string, string>
        {
            { "NULL" , "000" }, // the value is not stored
            { "M" , "001" }, // RAM[A]
            { "D" , "010" }, // D register
            { "MD" , "011" }, // RAM[A] and D register
            { "A" , "100" }, // A register
            { "AM" , "101" }, // A register and RAM[A]
            { "AD" , "110" }, // A register and D register
            { "AMD" , "111" }, // A register, RAM[A] and D register
        };
        private static readonly Dictionary<string, string> _jump = new Dictionary<string, string>
        {
            { "NULL", "000" }, // no jump
            { "JGT" , "001" }, // if out > 0 jump
            { "JEQ" , "010" }, // if out = 0 jump
            { "JGE" , "011" }, // if out >= 0 jump
            { "JLT" , "100" }, // if out < 0 jump
            { "JNE" , "101" }, // if out != 0 jump
            { "JLE" , "110" }, // if out <= 0 jump
            { "JMP" , "111" }, // unconditional jump
        };
        private static Dictionary<string, int> SymbolTable = new Dictionary<string, int> 
        {
            // pre-defined symbols
            { "R0", 0 },
            { "R1", 1 },
            { "R2", 2 },
            { "R3", 3 },
            { "R4", 4 },
            { "R5", 5 },
            { "R6", 6 },
            { "R7", 7 },
            { "R8", 8 },
            { "R9", 9 },
            { "R10", 10 },
            { "R11", 11 },
            { "R12", 12 },
            { "R13", 13 },
            { "R14", 14 },
            { "R15", 15 },
            { "SCREEN", 16384 },
            { "KBD", 24576 },
            { "SP", 0 },
            { "LCL", 1 },
            { "ARG", 2 },
            { "THIS", 3 },
            { "THAT", 4 },
        };
        private static int VariableCounter = 16;

        public static string Parse(string line)
        {
            var newLine = line.Contains("//")
                ? line.Substring(0, line.IndexOf("//") - 1).Trim()
                : line.Trim();

            if (newLine.StartsWith('@'))
                return ParseAInstruction(newLine);
            else
                return ParseCInstruction(newLine);
        }
        private static string ParseCInstruction(string newLine)
        {
            var dest = GetDest(newLine);
            var comp = GetComp(newLine);
            var jump = GetJump(newLine);

            var binaryDest = GetBinaryValue_C(dest, C_InstructionType.Dest);
            var binaryComp = GetBinaryValue_C(comp, C_InstructionType.Comp);
            var binaryJump = GetBinaryValue_C(jump, C_InstructionType.Jump);

            return "111" + binaryComp + binaryDest + binaryJump;
        }
        private static string ParseAInstruction(string line)
        {
            var value = line.Substring(1);

            //pre-defined symbol
            if (SymbolTable.TryGetValue(value, out int symbolValue))
                return GetBinaryValue_A(symbolValue);

            //integer
            else if (int.TryParse(value, out int intValue))
                return GetBinaryValue_A(intValue);

            //variable
            else
            {
                SymbolTable.Add(value, VariableCounter);
                var parsedLine = GetBinaryValue_A(VariableCounter);
                VariableCounter++;
                return parsedLine;  
            }
        }
        private static string GetBinaryValue_A(int value)
        {
            var binaryValue = Convert.ToString(value, 2).PadLeft(15, '0');
            return '0' + binaryValue;
        }
        private static string GetBinaryValue_C(string instruction, C_InstructionType instructionType)
        {
            string returnValue = string.Empty;
            switch (instructionType)
            {
                case C_InstructionType.Dest:
                    _dest.TryGetValue(instruction, out string destValue);
                    returnValue = destValue;
                    break;

                case C_InstructionType.Comp:
                    _comp.TryGetValue(instruction, out string compValue);
                    returnValue = compValue;
                    break;

                case C_InstructionType.Jump:
                    _jump.TryGetValue(instruction, out string jumpValue);
                    returnValue = jumpValue;
                    break;
            }

            return returnValue;
        }
        private static string GetDest(string line)
        {
            return line.Contains('=')
                ? line.Substring(0, line.IndexOf('=')).Trim()
                : "NULL";
        }
        private static string GetComp(string line)
        {
            var comp = string.Empty;
            int from;
            int to;

            if (line.Contains('=') && line.Contains(';'))
            {
                from = line.IndexOf('=') + 1;
                to = line.IndexOf(';');
                comp = line.Substring(from, to - from);
            }

            if (line.Contains(';'))
            {
                to = line.IndexOf(';');
                comp = line.Substring(0, to);
            }

            if (line.Contains('='))
            {
                from = line.IndexOf('=') + 1;
                comp = line.Substring(from);
            }

            return comp.Trim();
        }
        private static string GetJump(string line)
        {
            var jumpLine = line.TrimEnd();
            var hasJump = jumpLine.Contains(';');

            if (!hasJump) return "NULL";

            int from = jumpLine.IndexOf(';') + 1;
            int to = jumpLine.Length;

            return jumpLine.Substring(from, to - from).Trim();
        }
        public static void AddSymbol(string line, int nextLine)
        {
            int from = line.IndexOf('(') + 1;
            int to = line.LastIndexOf(')');
            var symbol = line.Substring(from, to - from);

            SymbolTable.Add(symbol, nextLine);
        }
    }

    public enum C_InstructionType 
    { 
        Dest = 0,
        Comp = 1,
        Jump = 2,
    }
}
