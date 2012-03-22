using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace D3Database
{
	class Program
	{
        /// <summary>
        /// Represents the account of the currently signed in user. Null if no user is signed in.
        /// </summary>
		static Account currentAccount = null;

        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.Title = "D3SharpDB CLI";
            try
            {
                string dbFile = "D3Sharp.db";
                string dbConnect = @"../../Data/" + dbFile;
                Database.Instance.Connect(dbConnect);
                if (Database.Instance.Connection.State != System.Data.ConnectionState.Open)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to open connection to: {0}", dbFile);
                    Console.ResetColor();
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Connected to {0}", dbFile);
                Console.WriteLine();
                PrintHelp();

                while (true)
                {
                    if (currentAccount != null)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(currentAccount.Name);
                        Console.ResetColor();
                    }
                    Console.Write(">>");
                    var command = Console.ReadLine();
                    switch (command)
                    {
                        case "exit":
                            return;
                        case "list accounts":
                            CommandListAccounts();
                            break;
                        case "create account":
                            CommandCreateAccount();
                            break;
                        case "login":
                            CommandLogin();
                            break;
                        case "logout":
                            CommandLogout();
                            break;
                        case "list heroes":
                            CommandListHeroes();
                            break;
                        case "create hero":
                            CommandCreateHero();
                            break;
                        case "create banner":
                            CommandCreateBanner();
                            break;
                        case "list banners":
                            CommandListBanners();
                            break;
                        case "hero level up":
                            CommandHeroLevelUp();
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Unknown command");
                            PrintHelp();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception:");
                Console.ResetColor();
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
            finally
            {
                try { Database.Instance.Connection.Close(); }
                catch { }
            }
        }

        /// <summary>
        /// Outputs a list of acceptable commands.
        /// </summary>
        static void PrintHelp()
        {
            Console.Write("Commands: ");
            Console.Write("exit, login, logout, list accounts, create account, create hero, list heroes, hero level up, clear" + Environment.NewLine);
        }

        /// <summary>
        /// Handles the "create account" command which attempts to create a new account in the database.
        /// </summary>
        static void CommandCreateAccount()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name.");
                Console.ResetColor();
                Console.Write("Name: ");
                name = Console.ReadLine();
            }

            string password;            
            Console.Write("Password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            password = Console.ReadLine();
            while (string.IsNullOrEmpty(password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid password.");
                Console.ResetColor();
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.Black;
                password = Console.ReadLine();
            }
            Console.ResetColor();

            string passwordConfirm;
            Console.Write("Confirm Password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            passwordConfirm = Console.ReadLine();
            while (passwordConfirm != password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Passwords don't match.");
                Console.ResetColor();
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.Black;
                password = Console.ReadLine();
                while (string.IsNullOrEmpty(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid password.");
                    Console.ResetColor();
                    Console.Write("Password: ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    password = Console.ReadLine();
                }
                Console.ResetColor();
                Console.Write("Confirm Password: ");
                Console.ForegroundColor = ConsoleColor.Black;
                passwordConfirm = Console.ReadLine();
                Console.ResetColor();
            }

            Console.Write("Gold: "); 
            string goldString = Console.ReadLine();
            int gold;
            while (int.TryParse(goldString, out gold))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid gold. Please choose a number.");
                Console.ResetColor();
                Console.Write("Gold: ");
                goldString = Console.ReadLine();
            }
            
            Console.Write("Gender (male/female): ");
            int gender = 0;
            while (gender == 0)
            {
                switch (Console.ReadLine().ToLower().Trim())
                {
                    case "male":
                    case "m":
                        gender = 1;
                        break;
                    case "female":
                    case "f":
                        gender = 2;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid gender. Please choose male or female.");
                        Console.ResetColor();
                        Console.Write("Gender (male/female): ");
                        break;
                }
            }

            Account account = new Account(name, gold, gender);
            if (account.Create(password))
            {
                Console.Write("Account ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(name);
                Console.ResetColor();
                Console.Write(" created.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An account with that name already exists");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "list accounts" command which lists all accounts in the database.
        /// </summary>
        static void CommandListAccounts()
        {
            SQLiteCommand command = new SQLiteCommand(string.Format("SELECT account_id, account_name FROM account ORDER BY account_id ASC"), Database.Instance.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("{0}: {1}", reader.GetInt32(0), reader.GetString(1));
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No accounts found in the database.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "login" command which attempts to log in as a specific user.
        /// </summary>
        static void CommandLogin()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine().Trim();
            Console.Write("Password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            string password = Console.ReadLine();
            Console.ResetColor();

            if (Account.Authorize(name, password, out currentAccount))
            {
                Console.Write("Logged in as ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(name);
                Console.ResetColor();
                Console.WriteLine(".");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to log in.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "logout" command which logs out the currently logged in user.
        /// </summary>
        static void CommandLogout()
        {
            if (currentAccount != null)
            {
                currentAccount = null;
                Console.WriteLine("Logged out.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Already logged out.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "list heroes" command which lists all heroes in the database associated with the currently logged in account.
        /// </summary>
        static void CommandListHeroes()
        {
            if (currentAccount == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not logged in.");
                Console.ResetColor();
                return; // exit
            }
            List<Hero> heroes = currentAccount.GetHeroes();
            if (heroes.Count > 0)
            {
                foreach (Hero hero in heroes)
                {
                    string classStr;
                    ConsoleColor classColour;

                    switch (hero.HeroClass)
                    {
                        case 1:
                            classStr = "Wizard";
                            classColour = ConsoleColor.Cyan;
                            break;
                        case 2:
                            classStr = "Witch Doctor";
                            classColour = ConsoleColor.Green;
                            break;
                        case 3:
                            classStr = "Demon Hunter";
                            classColour = ConsoleColor.Magenta;
                            break;
                        case 4:
                            classStr = "Monk";
                            classColour = ConsoleColor.Yellow;
                            break;
                        case 5:
                            classStr = "Barbarian";
                            classColour = ConsoleColor.Red;
                            break;
                        default:
                            classStr = "Unknown";
                            classColour = ConsoleColor.Gray;
                            break;
                    }
                    string genderStr;

                    switch (hero.Gender)
                    {
                        case 1:
                            genderStr = "male";
                            break;
                        case 2:
                            genderStr = "female";
                            break;
                        default:
                            genderStr = "unknown";
                            break;
                    }

                    Console.Write(hero.Id.ToString().PadLeft(3, '0'));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" | ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(hero.Name.PadRight(12));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" | ");
                    Console.ResetColor();
                    Console.Write("level " + hero.Level.ToString().PadLeft(3, '0'));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" | ");
                    Console.ResetColor();
                    Console.Write(genderStr.PadRight(6));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" | ");
                    Console.ForegroundColor = classColour;
                    Console.Write(classStr.PadRight(12));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" | ");
                    Console.ResetColor();
                    Console.WriteLine(" " + hero.Experience.ToString() + " XP");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No heroes associated with this account.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "create hero" command which creates a hero under the currently logged in account and saves it to the database.
        /// </summary>
        static void CommandCreateHero()
        {
            if (currentAccount == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not logged in.");
                Console.ResetColor();
                return; // exit
            }
            Console.Write("Name: ");
            string name = Console.ReadLine();
            while (Regex.IsMatch(name, @"\d"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name. Names cannot contain numbers.");
                Console.ResetColor();
                Console.Write("Name: ");
                name = Console.ReadLine();
            }

            while (name.Length < 3 || name.Length > 12)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name. Names must be between 3 and 12 characters inclusive.");
                Console.ResetColor();
                Console.Write("Name: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("Hero Class:\n1: Wizard\n2: Witch Doctor\n3: Demon Hunter\n4: Monk\n5: Barbarian");
            string heroClassString = Console.ReadLine();
            int heroClass;
            while (!int.TryParse(heroClassString, out heroClass) || heroClass > 5 || heroClass < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid class. Please choose a number.");
                Console.ResetColor();
                Console.WriteLine("Hero Class:\n1: Wizard\n2: Witch Doctor\n3: Demon Hunter\n4: Monk\n5: Barbarian");
                heroClassString = Console.ReadLine();
            }

            Console.Write("Gender (male/female): ");
            int gender = 0;
            while (gender == 0)
            {
                switch (Console.ReadLine().ToLower().Trim())
                {
                    case "male":
                    case "m":
                        gender = 1;
                        break;
                    case "female":
                    case "f":
                        gender = 2;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid gender. Please choose male or female.");
                        Console.ResetColor();
                        Console.Write("Gender (male/female): ");
                        break;
                }
            }

            Console.Write("Level: ");
            string levelString = Console.ReadLine();
            int level;
            while (!int.TryParse(levelString, out level))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid level. Please choose a number.");
                Console.ResetColor();
                Console.Write("Level: ");
                levelString = Console.ReadLine();
            }

            Console.Write("Experience: ");
            string experienceString = Console.ReadLine();
            int experience;

            // check that input is numeric
            while (!int.TryParse(experienceString, out experience))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid experience. Please choose a number.");
                Console.ResetColor();
                Console.Write("Experience: ");
                experienceString = Console.ReadLine();
            }

            
            Hero hero = new Hero(currentAccount.Id, name, heroClass, gender, experience, level);
            if (hero.Create(currentAccount.Id))
            {
                Console.Write("Hero ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(name);
                Console.ResetColor();
                Console.WriteLine(" created.");
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hero with that name already exists");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "create banner" command which creates a banner for the currently logged in account.
        /// </summary>
        static void CommandCreateBanner()
        {
            if (currentAccount == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not logged in");
                Console.ResetColor();
                return; // exit
            }

            Console.Write("Background Colour: ");
            string backgroundColorString = Console.ReadLine();
            int backgroundColor;
            while (int.TryParse(backgroundColorString, out backgroundColor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid background color. Please choose a number.");
                Console.ResetColor();
                Console.Write("Background Colour: ");
                backgroundColorString = Console.ReadLine();
            }
          
            Console.Write("Banner: ");
            string bannerString = Console.ReadLine();
            int banner;
            while (int.TryParse(bannerString, out banner))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid banner. Please choose a number.");
                Console.ResetColor();
                Console.Write("Banner: ");
                bannerString = Console.ReadLine();
            }

            Console.Write("Pattern: ");
            string patternString = Console.ReadLine();
            int pattern;
            while (int.TryParse(patternString, out pattern))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid pattern. Please choose a number.");
                Console.ResetColor();
                Console.Write("Pattern: ");
                patternString = Console.ReadLine();
            }

           Console.Write("Pattern Colour: ");
            string patternColorString = Console.ReadLine();
            int patternColor;
            while (int.TryParse(patternColorString, out patternColor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid pattern colour. Please choose a number.");
                Console.ResetColor();
                Console.Write("Pattern Colour: ");
                patternColorString = Console.ReadLine();
            }
          
            Console.Write("Placement: ");
            string placementString = Console.ReadLine();
            int placement;
            while (int.TryParse(placementString, out placement))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid placement. Please choose a number.");
                Console.ResetColor();
                Console.Write("Placement: ");
                placementString = Console.ReadLine();
            }
         
            Console.Write("Sigil: ");
            string sigilMainString = Console.ReadLine();
            int sigilMain;
            while (int.TryParse(sigilMainString, out sigilMain))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid sigil. Please choose a number.");
                Console.ResetColor();
                Console.Write("Sigil: ");
                sigilMainString = Console.ReadLine();
            }
           
            Console.Write("Sigil Accent: ");
            string sigilAccentString = Console.ReadLine();
            int sigilAccent;
            while (int.TryParse(sigilAccentString, out sigilAccent))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid sigil accent. Please choose a number.");
                Console.ResetColor();
                Console.Write("Sigil Accent: ");
                sigilAccentString = Console.ReadLine();
            }
         
            Console.Write("Sigil Colour: ");
            string sigilColorString = Console.ReadLine();
            int sigilColor;
            while (int.TryParse(sigilColorString, out sigilColor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid sigil colour. Please choose a number.");
                Console.ResetColor();
                Console.Write("Sigil Colour: ");
                sigilColorString = Console.ReadLine();
            }
        
            Console.Write("Use Sigil Variant (yes/no): ");
            string useSigilVariantString = Console.ReadLine();
            bool useSigilVariant = false;
            bool useSigilVariantSet = false;

            while (!useSigilVariantSet)
            {
                switch (useSigilVariantString.ToLower())
                {
                    case "yes":
                    case "y":
                        useSigilVariant = true;
                        useSigilVariantSet = true;
                        break;
                    case "no":
                    case "n":
                        useSigilVariant = false;
                        useSigilVariantSet = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid sigil colour. Please choose a number.");
                        Console.ResetColor();
                        Console.Write("Use Sigil Variant (yes/no): ");
                        useSigilVariantString = Console.ReadLine();
                        break;
                }
            }
          
            AccountBanner accountBanner = new AccountBanner(currentAccount.Id, backgroundColor, banner, pattern, 
                                                                                                patternColor, placement, sigilAccent, sigilMain, 
                                                                                                sigilColor, useSigilVariant);
            if (accountBanner.Create())
            {
                Console.WriteLine("Banner created");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to create banner.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "list banners" command which lists all the banners associated with the currently logged in account.
        /// </summary>
        static void CommandListBanners()
        {
            if (currentAccount == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not logged in.");
                Console.ResetColor();
                return; // exit
            }
            
            List<AccountBanner> banners = currentAccount.GetBanners();

            if (banners.Count > 0)
            {
                foreach (AccountBanner banner in banners)
                {
                    Console.WriteLine(banner);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No banners associated with this account.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Handles the "hero level up" command which increments a hero's level.
        /// </summary>
        static void CommandHeroLevelUp()
        {
            if (currentAccount == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not logged in");
                Console.ResetColor();
                return; // exit
            }

            Console.Write("Hero id: ");
            string heroIdString = Console.ReadLine();
            int heroId;
            if (!int.TryParse(heroIdString, out heroId) || heroId == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid hero id.");
                Console.ResetColor();
                return; // exit
            }

            Hero hero;
            if (!Hero.Load(heroId, out hero))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hero with the specified id exists.");
                Console.ResetColor();
                return; // exit
            }

            if (hero.AccountId != currentAccount.Id)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Specified hero is not associated with this account.");
                Console.ResetColor();
                return; // exit
            }
            hero.Level++;
            hero.Save();
            Console.Write("Hero ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(hero.Name);
            Console.ResetColor();
            Console.Write(" is now level ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(hero.Level);
            Console.ResetColor();
            Console.WriteLine(".");
        }
	}
}
