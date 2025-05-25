using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    // Dictionary to match keywords to numeric inputs.
        static readonly Dictionary<string, string> numberToKeyword = new Dictionary<string, string>
        {
            {"1", "social"},
            {"2", "phishing"},
            {"3", "password"},
            {"4", "scam"},
            {"5", "wi-fi"},
            {"6", "device"},
            {"7", "safety"},
            {"8", "convo"},
            {"9", "privacy"},
            {"10", "update"},
            {"11", "malware"},
            {"12", "vpn"},
            {"13", "backup"},
            {"14", "2fa"},
            {"15", "permission"}
        };

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
            var rng = new Random();

            var keywordTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "social", new List<string>
            {
                "Keep your social media profiles private to avoid identity theft.",
                "Think before you post. Once it's online, it's out there forever.",
                "Don’t overshare personal details like your location or contact info."
            }
        },
        { "phishing", new List<string>
            {
                "Phishing is when someone pretends to be trustworthy to steal your info. Always check where emails come from.",
                "Be cautious of emails asking for personal information—scammers disguise themselves as trusted sources.",
                "Don’t rush to click links. Hover over them first to check where they lead."
            }
        },
        { "password", new List<string>
            {
                "Use strong passwords with a mix of letters, numbers, and symbols.",
                "Avoid reusing the same password on multiple accounts.",
                "Consider using a password manager to generate and store your passwords."
            }
        },
        { "scam", new List<string>
            {
                "If it sounds too good to be true, it probably is.",
                "Stick to trusted websites and don’t give away info to strangers online.",
                "Always verify the legitimacy of offers and requests before taking action."
            }
        },
        { "wi-fi", new List<string>
            {
                "Public Wi-Fi isn’t always safe. Avoid logging into sensitive accounts.",
                "Use a VPN to encrypt your data on public networks.",
                "Turn off auto-connect for open networks when you’re out."
            }
        },
        { "device", new List<string>
            {
                "Keep your phone and computer updated regularly.",
                "Avoid installing random apps from untrusted sources.",
                "Use screen locks and antivirus software for extra protection."
            }
        },
        { "safety", new List<string>
            {
                "Trust your instincts—if something feels off, it probably is.",
                "Slow down and double-check before clicking or sharing online.",
                "Cybersecurity is about awareness—stay alert!"
            }
        },
        { "privacy", new List<string>
            {
                "Check app and browser settings to control what you share.",
                "Only give apps the permissions they truly need.",
                "Use private browsing when researching sensitive topics."
            }
        },
        { "update", new List<string>
            {
                "Don’t skip updates—they fix security bugs and keep you protected.",
                "Enable auto-updates for your OS and apps to stay current.",
                "Software updates often patch vulnerabilities hackers exploit."
            }
        },
        { "malware", new List<string>
            {
                "Avoid downloading pirated software—it often carries malware.",
                "Install antivirus software and keep it updated.",
                "Think twice before opening unknown email attachments."
            }
        },
        { "vpn", new List<string>
            {
                "A VPN encrypts your connection and hides your IP address.",
                "Use a VPN on public Wi-Fi to protect your data.",
                "VPNs help prevent tracking and improve online privacy."
            }
        },
        { "backup", new List<string>
            {
                "Back up your files regularly to avoid data loss.",
                "Use both cloud and local backups for safety.",
                "Test your backups occasionally to ensure they work."
            }
        },
        { "2fa", new List<string>
            {
                "Enable Two-Factor Authentication on all important accounts.",
                "2FA adds an extra layer of security even if your password is leaked.",
                "Use authenticator apps for more secure 2FA than SMS."
            }
        },
        { "permission", new List<string>
            {
                "Apps don’t need access to everything—review their permissions.",
                "Turn off microphone or camera access if not needed.",
                "Remove app permissions you don’t recognize or use."
            }
        },
        { "convo", new List<string>
            {
                "Let’s talk! Try asking about privacy, scams, or any online danger.",
                "I’m here to chat and help you stay secure online.",
                "Want a tip or just some cyber-chitchat? I’ve got you."
            }
        },
        { "conversation", new List<string>
            {
                "Let’s talk! Try asking about privacy, scams, or any online danger.",
                "I’m here to chat and help you stay secure online.",
                "Want a tip or just some cyber-chitchat? I’ve got you."
            }
        },
        { "how are you", new List<string>
            {
                "I’m doing great and ready to help you stay safe online!",
                "Always vigilant, always cyber-secure!",
                "Feeling firewalled and fabulous—thanks for asking!"
            }
        },
        { "what can i ask you", new List<string>
            {
                "You can ask about scams, privacy, social media, VPNs and more.",
                "Ask me about staying safe online, phishing, passwords—anything cybersecurity.",
                "I’ve got tips on malware, backups, updates, and way more!"
            }
        },
        { "what can i ask", new List<string>
            {
                "You can ask about scams, privacy, social media, VPNs and more.",
                "Ask me about staying safe online, phishing, passwords—anything cybersecurity.",
                "I’ve got tips on malware, backups, updates, and way more!"
            }
        },
        { "what's your purpose", new List<string>
            {
                "I’m here to help you stay safe and smart while using the internet.",
                "My purpose? Keeping you one step ahead of cyber threats.",
                "Guiding you through the digital world securely—that’s what I do!"
            }
        },
        { "what is your purpose", new List<string>
            {
                "I’m here to help you stay safe and smart while using the internet.",
                "My purpose? Keeping you one step ahead of cyber threats.",
                "Guiding you through the digital world securely—that’s what I do!"
            }
        },
        { "what do you do", new List<string>
            {
                "I give you cybersecurity tips to stay safe online.",
                "I share advice to protect your data, privacy, and devices.",
                "I help you spot scams, dodge malware, and browse smart."
            }
        },
        { "purpose", new List<string>
            {
                "I’m here to help you stay safe and smart while using the internet.",
                "My purpose? Keeping you one step ahead of cyber threats.",
                "Guiding you through the digital world securely—that’s what I do!"
            }
        }
    };

            // Try to convert numeric input to keyword
            string matchedKeyword = null;
            if (numberToKeyword.ContainsKey(input))
            {
                matchedKeyword = numberToKeyword[input];
            }
            else
            {
                // Otherwise check if any keyword is contained in input string
                foreach (var keyword in keywordTips.Keys)
                {
                    if (input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchedKeyword = keyword;
                        break;
                    }
                }
            }

            if (matchedKeyword != null)
            {
                var tips = keywordTips[matchedKeyword];
                // Pick a random tip to reply with
                string response = tips[rng.Next(tips.Count)];
                Console.ForegroundColor = ConsoleColor.Cyan;
                PrintWithEffect($"Cypher: {response}");
                Console.ResetColor();
            }
            else
            {
                // Handle unexpected input with a random generic response
                var randomResponses = new List<string>
                {
                    "I didn’t quite catch that. Could you try another topic?",
                    "Let’s try a different question. Type 'help' to see topics.",
                    "Hmm, that’s new to me! Ask about cybersecurity or type 'help'."
                };
                string response = randomResponses[rng.Next(randomResponses.Count)];
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"Cypher: {response}");
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