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
            // Set the console window title
            Console.Title = "Cypher - Your Cybersecurity Companion";

            // Method to display the Cypher banner/logo
            PrintBanner();

            // Method to play an introduction audio
            PlayIntroductionAudio("Cypher Chatbot.wav");

            // Method that asks for the user's name and welcomes them
            AskNameAndGreet(out string userName);  

            // Continuous loop to keep the use engaged
            while (true)
            {
                //Set color
                Console.ForegroundColor = ConsoleColor.Cyan;
                // Prompt user for input
                PrintWithEffect($"\nCypher: {userName}, how can I assist you? ");  

                Console.ForegroundColor = ConsoleColor.Yellow;
                // Read input and trim it
                string input = Console.ReadLine()?.ToLower().Trim();  
                Console.ResetColor();

                // If the input is empty, user is prompted for valid input
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintWithEffect("Cypher: Please enter a valid input.");
                    Console.ResetColor();
                    continue;
                }

                // Entering Exit breaks the loop and leave the chatbot safetly
                if (input == "exit")
                {
                    DrawDivider("GOODBYE");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    PrintWithEffect("\nCypher: Stay secure out there!");  
                    Console.ResetColor();
                    break;
                }

                // Entering Help calls the HandleHelpMenu
                if (input == "help")
                {
                    HandleHelpMenu();
                    continue;
                }

                // Method to handle user queries 
                HandleUserQuery(input, userName);
            }
        }

        // Displays the banner with ASCII art
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

        // Plays an audio file as an introduction
        static void PlayIntroductionAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (File.Exists(fullPath))
                {
                    // Play audio synchronously
                    new SoundPlayer(fullPath).PlaySync();  
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    // If audio is not found, display an error
                    PrintWithEffect($"Audio file not found: {filePath}");  
                    Console.ResetColor();
                }
            }
            // Catch any errors when playing the audio
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"Error playing audio: {ex.Message}");  
                Console.ResetColor();
            }
        }

        // Method to ask for the user's name and greet them
        static void AskNameAndGreet(out string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintWithEffect("Cypher: What’s your name? ");  
            Console.ForegroundColor = ConsoleColor.White;
            // Read and store the user's name
            userName = Console.ReadLine()?.Trim();
            // Draw divider for the welcome message
            DrawDivider("WELCOME"); 

            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintWithEffect($"\nHello, {userName}! I’m Cypher, your online safety buddy. Let’s keep the web safe together!\n\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            // This method is called to display the main topics. 
            ShowMainTopics();  

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("\nType 'help' for help, or 'exit' to leave the chat anytime.");
            Console.ResetColor();
        }

        // Display a list of topics for the user to choose from
        static void ShowMainTopics()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintWithEffect(@"
Topics you can ask me about:
  [1] Social Media Safety  
  [2] Phishing Awareness  
  [3] Strong Passwords  
  [4] Avoiding Scams  
  [5] Wi-Fi Security  
  [6] Device Protection  
  [7] Staying Safe Online  
  [8] Chat with Me!  
  [9] Privacy Settings  
  [10] Software Updates  
  [11] Malware Protection  
  [12] Using a VPN  
  [13] Backing Up Your Data  
  [14] Two-Factor Authentication  
  [15] App Permissions
");
            Console.ResetColor();
        }

        // Display the help menu with main topics and usage tips. 
        static void HandleHelpMenu()
        {
            DrawDivider("HELP MENU");

            Console.ForegroundColor = ConsoleColor.Yellow;
            ShowMainTopics();
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintWithEffect(@"Other Commands:
  help   - Show this menu again
  exit   - Leave Cypher’s safe zone");

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("\nTip: Type a number or keyword like 'phishing' to learn more.");
            Console.ResetColor();
        }

        // Handle user input by matching it to queries
        static void HandleUserQuery(string input, string userName)
        {
            // A dictionary to map user-friendly input to response topics
            var topics = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "social", "Think twice before sharing personal stuff online. Keep your profiles private and steer clear of sketchy links—even if they come from friends." },
                { "phishing", "Phishing is when someone pretends to be trustworthy to steal your info. Always check where emails come from, and don’t rush to click!" },
                { "passwords", "Use strong passwords with a mix of letters, numbers, and symbols. Try not to reuse them, and a password manager can really help." },
                { "scams", "If it sounds too good to be true, it probably is. Stick to trusted websites and don’t give away info to strangers online." },
                { "wi-fi", "Public Wi-Fi isn’t always safe. Avoid logging into sensitive accounts, or use a VPN if you really need to connect." },
                { "devices", "Keep your phone and computer updated, avoid random apps, and use screen locks and antivirus to stay safe." },
                { "safety", "Trust your gut! If something feels off, it probably is. Better to slow down and double-check than take a risk." },
                { "privacy", "Check your app and browser settings. Give apps only the permissions they really need, nothing extra." },
                { "updates", "Don’t skip updates—they fix security bugs and help keep your devices protected from new threats." },
                { "malware", "Avoid shady downloads or pirated software. A good antivirus and a little caution go a long way." },
                { "vpn", "A VPN keeps your internet use private, especially on public Wi-Fi. It’s like a safety tunnel for your data." },
                { "backup", "Back up your files just in case something goes wrong. It's a lifesaver if your device crashes or gets hacked." },
                { "2fa", "Two-Factor Authentication adds an extra layer of security. It’s a small step that makes a big difference." },
                { "permissions", "Apps don’t need access to everything! Review permissions and turn off anything that seems unnecessary." },
                { "convo", "Feel free to chat! Try asking things like 'how are you', 'what’s your purpose', or 'what can I ask you'." },
                { "conversation", "Feel free to chat! Try asking things like 'how are you', 'what’s your purpose', or 'what can I ask you'." },
                { "how are you", "I’m doing great and ready to help you stay safe online!" },
                { "what can i ask you", "You can ask about stuff like social media safety, scams, passwords, or just type 'help' to see more." },
                { "what can i ask", "You can ask about stuff like social media safety, scams, passwords, or just type 'help' to see more." },
                { "what's your purpose", "I’m here to help you stay safe and smart while using the internet." },
                { "what is your purpose", "I’m here to help you stay safe and smart while using the internet." },
                { "what do you do", "I share tips on how to protect yourself online and avoid the bad stuff." },
                { "purpose", "I’m here to help you stay safe and smart while using the internet." }
            };

            // If the input is a number, convert it to the corresponding string
            var numberMap = new Dictionary<string, string>
            {
                { "1", "social" },
                { "2", "phishing" },
                { "3", "passwords" },
                { "4", "scams" },
                { "5", "wi-fi" },
                { "6", "devices" },
                { "7", "safety" },
                { "8", "conversation" },
                { "9", "privacy" },
                { "10", "updates" },
                { "11", "malware" },
                { "12", "vpn" },
                { "13", "backup" },
                { "14", "2fa" },
                { "15", "permissions" },
                { "16", "help" }
            };

            // If the input matches a number, map it to the relevant topic
            if (numberMap.TryGetValue(input, out string topic))
            {
                input = topic;
            }

            // If the topic is found, print the response
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
                // If the topic is not found validation is hadnled gracefully
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"\nCypher: I’m not sure about that, {userName}. Type 'help' to see your options.");
                Console.ResetColor();
            }
        }

        // Draw a divider line with a title, organizes conversations and seperates them
        static void DrawDivider(string title = "")
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (!string.IsNullOrWhiteSpace(title))
                Console.WriteLine($"\n==== {title.ToUpper()} =============================");
            else
                Console.WriteLine("\n===================================================");
            Console.ResetColor();
        }

        // Print text with a typing effect
        static void PrintWithEffect(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                // Introduce a small delay between characters to imitating human typing 
                Thread.Sleep(delay);  
            }
        }
    }
}
