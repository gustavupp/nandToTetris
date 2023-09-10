using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator
{
    internal class Writer
    {
        private static string C_Pop = "c_pop";
        private static string C_Push = "c_push";
        public static void WritePushPop(string commandType, string segment, int index)
        {
            var parsedCode = new List<string>();

            if (segment == "constant")
            {
                //*SP = i, SP++
                return;
            }
            
            //addr = segment + i
            parsedCode.Add($"@{index}");
            parsedCode.Add($"D=M");
            parsedCode.Add($"@{segment}");
            parsedCode.Add($"D=D+M");
            parsedCode.Add($"@addr"); //check if addr needs to be unique
            parsedCode.Add($"M=D");

            if (commandType == C_Push)
            {
                //*SP = *addr, SP++
                parsedCode.Add($"@addr");
                parsedCode.Add($"D=M");
                parsedCode.Add($"@SP");
                parsedCode.Add($"M=D");
                parsedCode.Add($"@SP");
                parsedCode.Add($"M=M+1");
            }
            else if (commandType == C_Pop)
            {
                //SP--, *addr = *SP
                parsedCode.Add($"@SP");
                parsedCode.Add($"M=M-1");
                parsedCode.Add($"@SP");
                parsedCode.Add($"A=M");
                parsedCode.Add($"D=M");
                parsedCode.Add($"@addr");
                parsedCode.Add($"M=D");
            }
        }
    }
}
