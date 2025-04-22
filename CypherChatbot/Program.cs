using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace CypherChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cypher - Your Cybersecurity Companion";
            PrintBanner();
        }

        static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintWithEffect(@"
_________                  .__                     
\_   ___ \  ___.__.______  |  |__    ____  _______ 
/    \  \/ <   |  |\____ \ |  |  \ _/ __ \ \_  __ \
\     \____ \___  ||  |_> >|   Y  \  ___/  |  | \/
 \______  / / ____||   __/ |___|  /\___  > |__|   
        \/  \/     |__|         \/     \/        
");
            Console.ResetColor();
        }

        static void DrawDivider(string title = "")
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (!string.IsNullOrWhiteSpace(title))
                Console.WriteLine($"\n==== {title.ToUpper()} =============================");
            else
                Console.WriteLine("\n===================================================");
            Console.ResetColor();
        }

        static void PrintWithEffect(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }
    }
}
