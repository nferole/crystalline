using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.ConsoleHelpers
{
    public static class CliExt
    {
        public static void WriteSameLine(string text)
        {
            Console.Write("\r"+new string(' ', Console.WindowWidth - 1)+"\r");
            Console.Write(text);
        }
        public static void WriteSameLineColor(string text, ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                Console.Write(text);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

    }
}
