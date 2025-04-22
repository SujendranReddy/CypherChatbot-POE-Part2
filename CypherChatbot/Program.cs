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

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                PrintWithEffect($"\nCypher: {userName}, how can I assist you? ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                string input = Console.ReadLine()?.ToLower().Trim();
                Console.ResetColor();

                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintWithEffect("Cypher: Please enter a valid input.");
                    Console.ResetColor();
                    continue;
                }

                if (input == "exit")
                {
                    DrawDivider("GOODBYE");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    PrintWithEffect("\nCypher: Stay secure out there!");
                    Console.ResetColor();
                    break;
                }

                if (input == "help")
                {
                    HandleHelpMenu();
                    continue;
                }

                HandleUserQuery(input, userName);
            }
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

        static void HandleHelpMenu()
        {
            DrawDivider("HELP MENU");

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintWithEffect(@"
Cypher: you can ask me about...

  [1] Social
  [2] Phishing
  [3] Passwords
  [4] Scams
  [5] Wi-Fi
  [6] Devices
  [7] Safety
  [8] Conversation
");

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintWithEffect(@"Other Commands:
  help   - Show this menu again
  exit   - Leave Cypher’s safe zone");

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("\nTip: Type a number or keyword like 'phishing' to learn more.");
            Console.ResetColor();
        }

        static void HandleUserQuery(string input, string userName)
        {
            var topics = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "social", "Limit what you share online, use private settings, and avoid clicking suspicious links—even from friends." },
                { "phishing", "Phishing tries to trick you into giving personal info. Check email sources carefully and don’t rush to click." },
                { "passwords", "Use a long password with letters, numbers, and symbols. Never reuse passwords and consider a password manager." },
                { "scams", "Be skeptical of deals that sound too good to be true. Check website URLs and avoid giving info on unknown sites." },
                { "wi-fi", "Avoid logging into sensitive accounts on public Wi-Fi. Use a VPN if you need to connect." },
                { "devices", "Keep your devices updated, don’t install unknown apps, and use screen locks and antivirus software." },
                { "safety", "Stay alert, trust your gut, and ask for help when unsure. It’s better to pause than to risk your info." },
                { "convo", "You can ask me qustions like 'how are you', 'what is your purpose', and 'what can i ask you'." },
                { "conversation", "You can ask me qustions like 'how are you', 'what is your purpose', and 'what can i ask you'." },
                { "how are you", "I’m running at optimal safety levels! Ready to assist." },
                { "what can i ask you", "Ask about social, scams, phishing or type 'help' for more." },
                { "what can i ask", "Ask about social, scams, phishing or type 'help' for more." },
                { "what's your purpose", "I exist to help you navigate the online world safely and confidently." },
                { "what is your purpose", "I exist to help you navigate the online world safely and confidently." },
                { "what do you do", "I guide people on safe digital habits and how to avoid online risks." },
                { "purpose", "I exist to help you navigate the online world safely and confidently." }
            };

            var numberMap = new Dictionary<string, string>
            {
                { "1", "social" },
                { "2", "phishing" },
                { "3", "passwords" },
                { "4", "scams" },
                { "5", "wi-fi" },
                { "6", "devices" },
                { "7", "safety" },
                { "8", "conversation" }
            };

            if (numberMap.TryGetValue(input, out string topic))
            {
                input = topic;
            }

            if (topics.TryGetValue(input, out string response))
            {
                Console.ForegroundColor = ConsoleColor.White;
                DrawDivider("RESPONSE");
                Console.ForegroundColor = ConsoleColor.Gray;
                PrintWithEffect($"\nCypher: {response}\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"\nCypher: I’m not sure about that, {userName}. Type 'help' to see your options.");
                Console.ResetColor();
            }
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
