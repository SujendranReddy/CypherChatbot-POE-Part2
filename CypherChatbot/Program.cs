using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Policy;
using System.Threading;

namespace CypherChatBot
{
    class Program
    {
        //Sets the current topic being discussed so user can asks for more tips on it.
        static string currentTopic = null;
        //Keeps track of tips used to prevent repeating tips. 
        static List<int> usedTipIndices = new List<int>();
        //Records users favorite topic.
        static string favoriteTopic = null;

        //Trigger words for sentiment detection. 
        static readonly List<string> sentimentKeywords = new List<string>
{
    "worried", "scared", "frustrated", "anxious", "confused", "upset",
    "angry", "nervous", "stressed", "overwhelmed", "afraid", "helpless",   
    "violated", "targeted", "hacked", "scammed", "tricked", "compromised",
    "exposed", "phished", "spied", "monitored", "leaked", "breached",   
    "locked out", "can't access", "account stolen", "identity stolen", "password leaked", 
    "paranoid", "suspicious", "distrustful", "uncertain", "clueless", "panicked"
};


        //Triggers words for more tips. 
        static readonly List<string> clarificationTriggers = new List<string> {
            "more details", "i don't understand", "dont understand", "explain",
            "can you elaborate", "i’m confused", "confused", "clarify", "what do you mean", "more"
        };

        // Variation of prompts to reduce reppetition. 
        static readonly List<string> promptVariants = new List<string>
{
    "How can I help you this time, {0}?",
    "Let’s talk.",
    "Got something you’d like to ask, {0}?",
    "What would you like to explore today, {0}?",
    "Cypher's ready—what do you need, {0}?",
    "Let’s get into it, {0}. What’s next?",
    "Here to help, {0}. Ask away!"
};

        //Method to retrieve random prompt.
        static string GetRandomPrompt(string userName)
        {
            var rand = new Random();
            return string.Format(promptVariants[rand.Next(promptVariants.Count)], userName);
        }
        static void Main(string[] args)
        {
            Console.Title = "Cypher - Your Cybersecurity Companion";

            PrintBanner();
            PlayIntroductionAudio("Cypher Chatbot.wav");
            AskNameAndGreet(out string userName);

            // After greeting, if favoriteTopic exists offer  tip via the method, 
            if (favoriteTopic != null)
            {
                FavoriteTopicTip();
            }

            while (true)
            {
                PrintWithEffect($"\nCypher: {GetRandomPrompt(userName)}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                string input = Console.ReadLine()?.ToLower().Trim();
                Console.ResetColor();

                if (string.IsNullOrEmpty(input))
                {
                    DrawDivider("RESPONSE");
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

                // Handle user query method 
                DrawDivider("RESPONSE");
                HandleUserQuery(input, userName);
            }
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
  [1] Social Media Safety  
  [2] Phishing Awareness  
  [3] Strong Passwords  
  [4] Avoiding Scams  
  [5] Wi-Fi Security  
  [6] Device Protection  
  [7] Staying Safe Online  
  [8] Privacy Settings  
  [9] Software Updates  
  [10] Malware Protection  
  [11] Using a VPN  
  [12] Backing Up Your Data  
  [13] Two-Factor Authentication  
  [14] App Permissions
");
            Console.ResetColor();
        }

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


        //Dictionaru containing definitions 
        static readonly Dictionary<string, string> topicDefinitions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    { "social media safety", "Social Media Safety involves protecting your personal information and managing privacy settings to avoid online threats." },
    { "phishing", "Phishing is a cyber attack where attackers try to trick you into giving personal information via fake emails or websites." },
    { "passwords", "Strong Passwords are long, unique, and use a mix of letters, numbers, and symbols to protect your accounts." },
    { "scam", "Avoiding Scams means staying cautious of online offers or messages that seem too good to be true or request personal data." },
    { "wi-fi security", "Wi-Fi Security ensures your wireless network is protected using strong passwords and encryption like WPA3." },
    { "device protection", "Device Protection refers to using antivirus software, keeping systems updated, and avoiding unsafe downloads." },
    { "staying safe online", "Staying Safe Online means practicing good habits like not oversharing, using secure websites, and avoiding suspicious links." },
    { "privacy", "Privacy Settings help control what information you share online and who can access it." },
    { "software updates", "Software Updates fix security bugs and improve the performance and safety of your apps and operating system." },
    { "malware", "Malware Protection defends against harmful software that can damage or control your device without permission." },
    { "vpn", "A VPN (Virtual Private Network) encrypts your internet connection, making your browsing more private and secure." },
    { "backing up your data", "Backing Up Your Data means saving copies of your files in case your device is lost, stolen, or crashes." },
    { "two-factor authentication", "Two-Factor Authentication adds an extra layer of security by requiring a second code in addition to your password." },
    { "app permissions", "App Permissions control what data and features an app can access on your device, like location or camera." }
};


        // Dictionary listing various topics. 
        static Dictionary<string, List<string>> keywordTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
{

    { "social", new List<string>
        {
            "Keep your social media profiles private to avoid identity theft.",
            "Think before you post. Once it's online, it's out there forever.",
            "Don’t overshare personal details like your location or contact info.",
            "Regularly review your friends/followers list and remove people you don’t know.",
            "Use strong, unique passwords for each social media account."
        }
    },
    { "phishing", new List<string>
        {
            "Phishing is when someone pretends to be trustworthy to steal your info. Always check where emails come from.",
            "Be cautious of emails asking for personal information—scammers disguise themselves as trusted sources.",
            "Don’t rush to click links. Hover over them first to check where they lead.",
            "Look for poor spelling or generic greetings—signs of phishing.",
            "Enable two-factor authentication to protect accounts even if credentials are compromised."
        }
    },
    { "password", new List<string>
        {
            "Use strong passwords with a mix of letters, numbers, and symbols.",
            "Avoid reusing the same password on multiple accounts.",
            "Consider using a password manager to generate and store your passwords.",
            "Change default passwords on new devices immediately.",
            "Use passphrases—long, memorable sentences—as passwords."
        }
    },
    { "scam", new List<string>
        {
            "If it sounds too good to be true, it probably is.",
            "Stick to trusted websites and don’t give away info to strangers online.",
            "Always verify the legitimacy of offers and requests before taking action.",
            "Check for secure URLs (https://) and padlock icons before entering details.",
            "Research a company or individual before sending money or personal data."
        }
    },
    { "wi-fi", new List<string>
        {
            "Public Wi-Fi isn’t always safe. Avoid logging into sensitive accounts.",
            "Use a VPN to encrypt your data on public networks.",
            "Turn off auto-connect for open networks when you’re out.",
            "Set your home network to WPA3 if available.",
            "Rename default SSIDs to deter attackers from guessing your router model."
        }
    },
    { "device", new List<string>
        {
            "Keep your phone and computer updated regularly.",
            "Avoid installing random apps from untrusted sources.",
            "Use screen locks and antivirus software for extra protection.",
            "Back up your device settings and data before major OS updates.",
            "Disable unused hardware features like Bluetooth when not in use."
        }
    },
    { "safety", new List<string>
        {
            "Trust your instincts—if something feels off, it probably is.",
            "Slow down and double-check before clicking or sharing online.",
            "Cybersecurity is about awareness—stay alert!",
            "Keep up with security news to know about emerging threats.",
            "Educate friends and family—security is stronger when everyone protects themselves."
        }
    },
    { "privacy", new List<string>
        {
            "Check app and browser settings to control what you share.",
            "Only give apps the permissions they truly need.",
            "Use private browsing when researching sensitive topics.",
            "Review privacy policies before signing up for new services.",
            "Use browser extensions to block trackers and ads."
        }
    },
    { "update", new List<string>
        {
            "Don’t skip updates—they fix security bugs and keep you protected.",
            "Enable auto-updates for your OS and apps to stay current.",
            "Software updates often patch vulnerabilities hackers exploit.",
            "Review change logs to understand what updates address.",
            "Schedule updates during off-hours to avoid interruptions."
        }
    },
    { "malware", new List<string>
        {
            "Avoid downloading pirated software—it often carries malware.",
            "Install antivirus software and keep it updated.",
            "Think twice before opening unknown email attachments.",
            "Use sandbox environments to test unknown files safely.",
            "Regularly scan external drives for hidden malware."
        }
    },
    { "vpn", new List<string>
        {
            "A VPN encrypts your connection and hides your IP address.",
            "Use a VPN on public Wi-Fi to protect your data.",
            "VPNs help prevent tracking and improve online privacy.",
            "Choose a VPN provider with a strict no-logs policy.",
            "Disconnect from the VPN when not needed to avoid unnecessary latency."
        }
    },
    { "backup", new List<string>
        {
            "Back up your files regularly to avoid data loss.",
            "Use both cloud and local backups for safety.",
            "Test your backups occasionally to ensure they work.",
            "Keep at least one offline backup to protect against ransomware.",
            "Automate backup schedules to reduce manual effort."
        }
    },
    { "2fa", new List<string>
        {
            "Enable Two-Factor Authentication on all important accounts.",
            "2FA adds an extra layer of security even if your password is leaked.",
            "Use authenticator apps for more secure 2FA than SMS.",
            "Don’t store backup codes in your inbox—keep them offline.",
            "Consider hardware keys (e.g., YubiKey) for critical accounts."
        }
    },
    { "permission", new List<string>
        {
            "Apps don’t need access to everything—review their permissions.",
            "Turn off microphone or camera access if not needed.",
            "Remove app permissions you don’t recognize or use.",
            "Revoke location access for apps that don’t require it.",
            "Check permissions again after each app update."
        }
    },
    { "convo", new List<string>
        {
            "Let’s talk! Try asking about privacy, scams, or any online danger.",
            "I’m here to chat and help you stay secure online.",
            "Want a tip or just some cyber-chitchat? I’ve got you.",
            "Feel free to ask about any security concern on your mind.",
            "What’s up? Need help with a specific cybersecurity issue?"
        }
    },
    { "conversation", new List<string>
        {
            "Let’s talk! Try asking about privacy, scams, or any online danger.",
            "I’m here to chat and help you stay secure online.",
            "Want a tip or just some cyber-chitchat? I’ve got you.",
            "Feel free to ask about any security concern on your mind.",
            "What’s up? Need help with a specific cybersecurity issue?"
        }
    },
    { "how are you", new List<string>
        {
            "I’m doing great and ready to help you stay safe online!",
            "Always vigilant, always cyber-secure!",
            "Feeling firewalled and fabulous—thanks for asking!",
            "I’m fine—just scanning for threats and ready to chat.",
            "All systems green! How can I assist you today?"
        }
    },
    { "what can i ask you", new List<string>
        {
            "You can ask about scams, privacy, social media, VPNs and more.",
            "Ask me about staying safe online, phishing, passwords—anything cybersecurity.",
            "I’ve got tips on malware, backups, updates, and way more!",
            "Try typing 'scams', 'passwords', or any topic on the list.",
            "Wondering what else? Type 'help' to see all commands."
        }
    },
    { "what can i ask", new List<string>
        {
            "You can ask about scams, privacy, social media, VPNs and more.",
            "Ask me about staying safe online, phishing, passwords—anything cybersecurity.",
            "I’ve got tips on malware, backups, updates, and way more!",
            "Try typing 'scams', 'passwords', or any topic on the list.",
            "Wondering what else? Type 'help' to see all commands."
        }
    },
};


        static void HandleUserQuery(string input, string userName)
        {
            // Sentiment detection
            string detectedEmotion = DetectSentiment(input);
            if (detectedEmotion != null)
            {
                RespondSentiment(detectedEmotion, input);
                return;
            }

            // Definition requests
            if (TryGetDefinition(input))
                return;

            // More details 
            if (clarificationTriggers.Any(phrase => input.Contains(phrase)))
            {
                if (currentTopic != null && keywordTips.ContainsKey(currentTopic))
                {
                    // provides the next unused tip for the last recorded topic
                    var tips = keywordTips[currentTopic];
                    var available = Enumerable.Range(0, tips.Count)
                                              .Where(i => !usedTipIndices.Contains(i))
                                              .ToList();
                    if (available.Count == 0)
                    {
                        // if all tips are used....
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        PrintWithEffect("Cypher: I've shared all I have on that topic. Want to ask about something else?");
                        Console.ResetColor();
                        currentTopic = null;
                        usedTipIndices.Clear();
                    }
                    else
                    {
                        int idx = new Random().Next(available.Count);
                        usedTipIndices.Add(available[idx]);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        PrintWithEffect($"Cypher: Sure, here's another tip on {currentTopic}: {tips[available[idx]]}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    //If a topic has not been selected......
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintWithEffect("Cypher: Could you clarify which topic you’d like more details on?");
                    Console.ResetColor();
                }
                return;
            }

            //Favorite‐topic
            if (TryDetectFavoriteTopic(input))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                PrintWithEffect($"Cypher: Got it! I’ll remember you’re interested in {favoriteTopic}.");
                Console.ResetColor();
                FavoriteTopicTip();
                return;
            }

            // IsBored method, contains triggerwords the user may use mid conversation this initializes the offerfacortietopictip.
            if (IsBored(input) && favoriteTopic != null)
            {
                FavoriteTopicTip();
                return;
            }

            // New topic selected
            string matchedTopic = GetMatchedTopic(input);
            if (matchedTopic != null)
            {
                currentTopic = matchedTopic;
                usedTipIndices.Clear();
                RandomTip(matchedTopic);
                return;
            }

            // Incase of spelling errors, etc.....
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("Cypher: Sorry, I don’t have info on that right now. Try asking about scams, privacy, or social media safety.");
            Console.ResetColor();
        }

        //Method for sentiment detection, scans for trigger words. 
        static string DetectSentiment(string input)
        {
            foreach (var word in sentimentKeywords)
            {
                if (input.Contains(word))
                    return word;
            }
            return null;
        }

        //Creates a response for sentiment when detected.
        static void RespondSentiment(string emotion, string input)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintWithEffect($"Cypher: I understand you’re feeling {emotion}. Cybersecurity can feel overwhelming, here’s a tip…");

            // Extracts topic keywords from input for a relevant tip
            string topicForTip = GetMatchedTopic(input);

            if (topicForTip != null)
            {
                RandomTip(topicForTip);
            }
            else if (favoriteTopic != null)
            {
                RandomTip(favoriteTopic);
            }
            else
            {
                // General tip from conversation incase of errors.
                RandomTip("convo");
            }
            Console.ResetColor();
        }

        //Method to detect favoritetopic
        static bool TryDetectFavoriteTopic(string input)
        {
            // Detects phrases like "I'm interested in scams", "My favorite topic is phishing", etc.
            string[] triggerWords = new[] { "favorite", "favourite", "interested in", "like", "love" };
            foreach (string trigger in triggerWords)
            {
                if (input.Contains(trigger))
                {
                    // Finds a topic keyword after trigger word
                    foreach (var topic in keywordTips.Keys)
                    {
                        if (input.Contains(topic))
                        {
                            favoriteTopic = topic;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //Trigger words for offerfavoritetopictip
        static bool IsBored(string input)
        {
            string[] bored = new[] { "hello", "hi", "hey", "greetings", "whats up", "ok", "cool","bored","boring","idk", "i dont know" };
            return bored.Any(g => input.Contains(g));
        }

        //Scans for topics.
        static string GetMatchedTopic(string input)
        {
            foreach (var topic in keywordTips.Keys)
            {
                if (input.Contains(topic))
                    return topic;
            }
            return null;
        }

        // Method to provide random tips without repeating.
        static void RandomTip(string topic)
        {
            if (!keywordTips.ContainsKey(topic))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"Cypher: Sorry, I don’t have tips for {topic} right now.");
                Console.ResetColor();
                return;
            }

            var tips = keywordTips[topic];
            var availableIndices = Enumerable.Range(0, tips.Count).Except(usedTipIndices).ToList();

            if (availableIndices.Count == 0)
            {
                // Resets if all tips used
                usedTipIndices.Clear();
                availableIndices = Enumerable.Range(0, tips.Count).ToList();
            }

            var random = new Random();
            int index = availableIndices[random.Next(availableIndices.Count)];
            usedTipIndices.Add(index);

            Console.ForegroundColor = ConsoleColor.Green;
            PrintWithEffect($"Cypher: {tips[index]}");
            Console.ResetColor();
        }

        //Prompts the user as to wether theyd like another tip of their favoruite topic.
        static void FavoriteTopicTip()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintWithEffect($"Cypher: Would you like another {favoriteTopic} tip? (yes/no)");

            Console.ForegroundColor = ConsoleColor.Yellow;
            string answer = Console.ReadLine()?.ToLower().Trim();
            Console.ResetColor();

            if (answer == "yes" || answer == "y")
            {
                RandomTip(favoriteTopic);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                PrintWithEffect("Cypher: No worries! Let me know if you want tips on anything else.");
                Console.ResetColor();
            }
        }

        //Method for definitions
        static bool TryGetDefinition(string input)
        {
            string[] definitionTriggers = { "what is", "define", "meaning of", "explain", "whats" };

            foreach (var trigger in definitionTriggers)
            {
                if (input.ToLower().Contains(trigger))
                {
                    foreach (var topic in topicDefinitions.Keys)
                    {
                        if (input.ToLower().Contains(topic.ToLower()))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            PrintWithEffect($"Cypher: {topicDefinitions[topic]}");
                            Console.ResetColor();
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        static void DrawDivider(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n" + new string('-', 40));
            Console.WriteLine($"--- {text} ---");
            Console.WriteLine(new string('-', 40));
            Console.ResetColor();
        }

        static void PrintWithEffect(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}








