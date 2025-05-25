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
        static string userInterestTopic = null;
        static List<int> usedTipIndices = new List<int>();

        static void Main(string[] args)
        {
            Console.Title = "Cypher - Your Cybersecurity Companion";
            PrintBanner();
            PlayIntroductionAudio("Cypher Chatbot.wav");
            AskNameAndGreet(out string userName);

            ShowMainTopics();
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintWithEffect("\nType 'help' for help, or 'exit' to leave the chat anytime.");
            Console.ResetColor();

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

        static void HandleUserQuery(string input, string userName)
        {
            var rng = new Random();
            var moreDetailsPhrases = new[]
            {
                "more details","more","details",
                "i don't understand","dont understand",
                "explain","can you elaborate",
                "i’m confused","im confused","confused",
                "clarify","what do you mean","help me"
            };

            var keywordTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"social", new List<string>{
                    "Keep your social media profiles private to avoid identity theft.",
                    "Think before you post. Once it's online, it's out there forever.",
                    "Don’t overshare personal details like your location or contact info."
                }},
                {"phishing", new List<string>{
                    "Phishing is when someone pretends to be trustworthy to steal your info. Always check where emails come from.",
                    "Be cautious of emails asking for personal information—scammers disguise themselves as trusted sources.",
                    "Don’t rush to click links. Hover over them first to check where they lead."
                }},
                {"password", new List<string>{
                    "Use strong passwords with a mix of letters, numbers, and symbols.",
                    "Avoid reusing the same password on multiple accounts.",
                    "Consider using a password manager to generate and store your passwords."
                }},
                {"scam", new List<string>{
                    "If it sounds too good to be true, it probably is.",
                    "Stick to trusted websites and don’t give away info to strangers online.",
                    "Always verify the legitimacy of offers and requests before taking action."
                }},
                {"wi-fi", new List<string>{
                    "Public Wi-Fi isn’t always safe. Avoid logging into sensitive accounts.",
                    "Use a VPN to encrypt your data on public networks.",
                    "Turn off auto-connect for open networks when you’re out."
                }},
                {"device", new List<string>{
                    "Keep your phone and computer updated regularly.",
                    "Avoid installing random apps from untrusted sources.",
                    "Use screen locks and antivirus software for extra protection."
                }},
                {"safety", new List<string>{
                    "Trust your instincts—if something feels off, it probably is.",
                    "Slow down and double-check before clicking or sharing online.",
                    "Cybersecurity is about awareness—stay alert!"
                }},
                {"privacy", new List<string>{
                    "Check app and browser settings to control what you share.",
                    "Only give apps the permissions they truly need.",
                    "Use private browsing when researching sensitive topics."
                }},
                {"update", new List<string>{
                    "Don’t skip updates—they fix security bugs and keep you protected.",
                    "Enable auto-updates for your OS and apps to stay current.",
                    "Software updates often patch vulnerabilities hackers exploit."
                }},
                {"malware", new List<string>{
                    "Avoid downloading pirated software—it often carries malware.",
                    "Install antivirus software and keep it updated.",
                    "Think twice before opening unknown email attachments."
                }},
                {"vpn", new List<string>{
                    "A VPN encrypts your connection and hides your IP address.",
                    "Use a VPN on public Wi-Fi to protect your data.",
                    "VPNs help prevent tracking and improve online privacy."
                }},
                {"backup", new List<string>{
                    "Back up your files regularly to avoid data loss.",
                    "Use both cloud and local backups for safety.",
                    "Test your backups occasionally to ensure they work."
                }},
                {"2fa", new List<string>{
                    "Enable Two-Factor Authentication on all important accounts.",
                    "2FA adds an extra layer of security even if your password is leaked.",
                    "Use authenticator apps for more secure 2FA than SMS."
                }},
                {"permission", new List<string>{
                    "Apps don’t need access to everything—review their permissions.",
                    "Turn off microphone or camera access if not needed.",
                    "Remove app permissions you don’t recognize or use."
                }},
                {"convo", new List<string>{
                    "Let’s talk! Try asking about privacy, scams, or any online danger.",
                    "I’m here to chat and help you stay secure online.",
                    "Want a tip or just some cyber-chitchat? I’ve got you."
                }},
                // small-talk
                {"how are you", new List<string>{
                    "I’m doing great and ready to help you stay safe online!",
                    "Always vigilant, always cyber-secure!",
                    "Feeling firewalled and fabulous—thanks for asking!"
                }},
                {"what can i ask you", new List<string>{
                    "You can ask about scams, privacy, social media, VPNs and more.",
                    "Ask me about staying safe online, phishing, passwords—anything cybersecurity.",
                    "I’ve got tips on malware, backups, updates, and way more!"
                }},
                {"what's your purpose", new List<string>{
                    "I’m here to help you stay safe and smart while using the internet.",
                    "My purpose? Keeping you one step ahead of cyber threats.",
                    "Guiding you through the digital world securely—that’s what I do!"
                }},
                {"what do you do", new List<string>{
                    "I give you cybersecurity tips to stay safe online.",
                    "I share advice to protect your data, privacy, and devices.",
                    "I help you spot scams, dodge malware, and browse smart."
                }},
                {"purpose", new List<string>{
                    "I’m here to help you stay safe and smart while using the internet.",
                    "My purpose? Keeping you one step ahead of cyber threats.",
                    "Guiding you through the digital world securely—that’s what I do!"
                }}
            };

            var numberToKeyword = new Dictionary<string, string>
            {
                {"1","social"},{"2","phishing"},{"3","password"},{"4","scam"},
                {"5","wi-fi"},{"6","device"},{"7","safety"},{"8","convo"},
                {"9","privacy"},{"10","update"},{"11","malware"},{"12","vpn"},
                {"13","backup"},{"14","2fa"},{"15","permission"}
            };

            if (userInterestTopic != null && moreDetailsPhrases.Any(p => input.Contains(p)))
            {
                var tips = keywordTips[userInterestTopic];
                var remaining = tips
                    .Select((tip, idx) => (tip, idx))
                    .Where(t => !usedTipIndices.Contains(t.idx))
                    .ToList();

                if (remaining.Any())
                {
                    var pick = remaining[rng.Next(remaining.Count)];
                    usedTipIndices.Add(pick.idx);

                    var followUpTemplates = new List<string>
                    {
                        $"{userName}, here’s another tip on {userInterestTopic}: {pick.tip}",
                        $"Absolutely, {userName}! One more thing to remember: {pick.tip}",
                        $"Good call, {userName}. Another tip for you: {pick.tip}",
                        $"Sure thing, {userName}. Don’t forget: {pick.tip}",
                        $"Right, {userName}? Here’s another nugget: {pick.tip}"
                    };

                    string response = followUpTemplates[rng.Next(followUpTemplates.Count)];
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintWithEffect($"Cypher: {response}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    PrintWithEffect(
                        "Cypher: Due to my demo model state, I do not have access to all information on this topic. You can try asking about something else!"
                    );
                    usedTipIndices.Clear();
                    userInterestTopic = null;
                }
                Console.ResetColor();
                return;
            }

            string matched = null;
            if (numberToKeyword.ContainsKey(input))
                matched = numberToKeyword[input];
            else
                matched = keywordTips.Keys
                    .FirstOrDefault(k => input.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

            if (matched != null)
            {
                userInterestTopic = matched;
                usedTipIndices.Clear();

                var tips = keywordTips[matched];
                int idx = rng.Next(tips.Count);
                usedTipIndices.Add(idx);

                string resp = $"{userName}, as someone interested in {matched}, it’s important to know: {tips[idx]}";
                Console.ForegroundColor = ConsoleColor.Cyan;
                PrintWithEffect($"Cypher: {resp}");
                Console.ResetColor();
            }
            else
            {
                var fallbacks = new[]
                {
                    "I didn’t quite catch that. Could you try another topic?",
                    "Let’s try a different question. Type 'help' to see topics.",
                    "Hmm, that’s new to me! Ask about cybersecurity or type 'help'."
                };
                Console.ForegroundColor = ConsoleColor.Red;
                PrintWithEffect($"Cypher: {fallbacks[rng.Next(fallbacks.Length)]}");
                Console.ResetColor();
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
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (File.Exists(fullPath))
                    new SoundPlayer(fullPath).PlaySync();
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
            Console.WriteLine();
        }
    }
}
