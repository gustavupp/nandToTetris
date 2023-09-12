using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator
{
    internal class Writer
    {
        public static void WritePushPop(CommandType commandType, string segment, int index)
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
            parsedCode.Add($"@addr");
            parsedCode.Add($"M=D");

            if (commandType == CommandType.C_Push)
            {
                //*SP = *addr, SP++
                parsedCode.Add($"@addr");
                parsedCode.Add($"D=M");
                parsedCode.Add($"@SP");
                parsedCode.Add($"M=D");
                parsedCode.Add($"@SP");
                parsedCode.Add($"M=M+1");
            }
            else if (commandType == CommandType.C_Pop)
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

        public static void WriteArithmetic(string command)
        {
            var parsedCode = new List<string>();
            parsedCode.Add($"// {command}");

            switch (command)
            {
                case "add":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("M=M+D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "sub":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("M=M-D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "and":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("M=M&D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "or":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("M=M|D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "neg":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@A=M-1");
                    parsedCode.Add("M=-M");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "not":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@A=M-1");
                    parsedCode.Add("M=!M");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "eq":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("D=M-D");

                    parsedCode.Add("@if_true");
                    parsedCode.Add("D;JEQ");

                    parsedCode.Add("D=0");
                    parsedCode.Add("@if_false");
                    parsedCode.Add("0;JMP");

                    parsedCode.Add("(if_true)");
                    parsedCode.Add("D=-1");

                    parsedCode.Add("(if_false)");
                    parsedCode.Add("@SP");
                    parsedCode.Add("A=M");
                    parsedCode.Add("M=D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "gt":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("D=M-D");

                    parsedCode.Add("@if_true");
                    parsedCode.Add("D;JGT");

                    parsedCode.Add("D=0");
                    parsedCode.Add("@if_false");
                    parsedCode.Add("0;JMP");

                    parsedCode.Add("(if_true)");
                    parsedCode.Add("D=-1");

                    parsedCode.Add("(if_false)");
                    parsedCode.Add("@SP");
                    parsedCode.Add("A=M");
                    parsedCode.Add("M=D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;

                case "lt":
                    parsedCode.Add("@SP");
                    parsedCode.Add("@AM=M-1");
                    parsedCode.Add("D=M");
                    parsedCode.Add("A=A-1");
                    parsedCode.Add("D=M-D");

                    parsedCode.Add("@if_true");
                    parsedCode.Add("D;JLT");

                    parsedCode.Add("D=0");
                    parsedCode.Add("@if_false");
                    parsedCode.Add("0;JMP");

                    parsedCode.Add("(if_true)");
                    parsedCode.Add("D=-1");

                    parsedCode.Add("(if_false)");
                    parsedCode.Add("@SP");
                    parsedCode.Add("A=M");
                    parsedCode.Add("M=D");
                    parsedCode.Add("@SP");
                    parsedCode.Add("M=M+1");
                    break;
            }
        }
    }

    public enum CommandType
    {
        C_Push,
        C_Pop,
    }
}
