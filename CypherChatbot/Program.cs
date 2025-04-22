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
            PlayIntroductionAudio("Cypher Chatbot.wav");
            AskNameAndGreet(out string userName);
        }

        static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintWithEffect(@"
_________                  .__                     
\_   ___ \  ___.__.______  |  |__    ____  _______ 
/    \  \/ <   |  |\____ \ |  |  \ _/ __ \ \_  __ \\
\     \____ \___  ||  |_> >|   Y  \  ___/  |  | \/
 \______  / / ____||   __/ |___|  /\___  > |__|   
        \/  \/     |__|         \/     \/        
");
            Console.ResetColor();
        }

        static void PlayIntroductionAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (File.Exists(fullPath))
                {
                    new SoundPlayer(fullPath).PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintWithEffect($"Audio file not found: {filePath}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"Error playing audio: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void AskNameAndGreet(out string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintWithEffect("Cypher: What’s your name? ");
            Console.ForegroundColor = ConsoleColor.White;
            userName = Console.ReadLine()?.Trim();

            DrawDivider("WELCOME");

            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintWithEffect($"\nHello, {userName}! I’m Cypher, your online safety buddy. Let’s keep the web safe together!\n\n");

            Console.ForegroundColor = ConsoleColor.Blue;
            ShowMainTopics();

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("\nType 'help' for help, or 'exit' to leave the chat anytime.");
            Console.ResetColor();
        }

        static void ShowMainTopics()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintWithEffect(@"
Topics you can ask me about:
  [1] Social
  [2] Phishing
  [3] Passwords
  [4] Scams
  [5] Wi-Fi
  [6] Devices
  [7] Safety
  [8] Conversation
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
