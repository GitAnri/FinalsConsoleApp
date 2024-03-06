using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FinalsConsoleApp.Program;

namespace FinalsConsoleApp
{
    public class ATM
    {

        static List<AtmUser> users = new List<AtmUser>();
        static string usersFilePath = "users.json";
        static string logFilePath = "log.json";

        public void AtmMenu()
        {
            LoadUsers();

            while (true)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RegisterUser();
                            break;
                        case 2:
                            Login();
                            break;
                        case 3:
                            SaveUsers();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
        static void RegisterUser()
        {
            Random rndpass = new Random();
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter your personal number: ");
            string personalNumber = Console.ReadLine();

            string password = rndpass.Next(1000, 9999).ToString();
            Console.Write("Your password will be: " + password);

            if (IsPersonalNumberUnique(personalNumber))
            {
                AtmUser newUser = new AtmUser
                {
                    Id = users.Count + 1,
                    Name = name,
                    LastName = lastName,
                    PersonalNumber = personalNumber,
                    Password = password,
                    Balance = 0
                };

                users.Add(newUser);
                Console.WriteLine("");
                Console.WriteLine("User registered successfully!");

                
                LogOperation($"User named {name} {lastName} registered on: {DateTime.Now}");
            }
            else
            {
                Console.WriteLine("Personal number is already in use. Please choose another one.");
            }
        }

        static void Login()
        {
            Console.Write("Enter your personal number: ");
            string personalNumber = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            
           AtmUser user = FindUser(personalNumber, password);

            if (user != null)
            {
                Console.WriteLine($"Welcome, {user.Name} {user.LastName}!");

                while (true)
                {
                    Console.WriteLine("1. Check balance");
                    Console.WriteLine("2. Deposit");
                    Console.WriteLine("3. Cash withdrawal");
                    Console.WriteLine("4. View history");
                    Console.WriteLine("5. Logout");
                    Console.Write("Select an option: ");

                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine($"Your current balance: {user.Balance} GEL");
                                LogOperation($"User named {user.Name} {user.LastName} checked the balance on: {DateTime.Now}");
                                break;
                            case 2:
                                Deposit(user);
                                break;
                            case 3:
                                Withdraw(user);
                                break;
                            case 4:
                                ViewHistory();
                                break;
                            case 5:
                                return;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Login failed. Personal number or password is incorrect.");
            }
        }

        static void Deposit(AtmUser user)
        {
            Console.Write("Enter the amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                user.Balance += amount;
                Console.WriteLine($"Deposited {amount} GEL. Your current balance: {user.Balance} GEL");
                LogOperation($"User named {user.Name} {user.LastName} filled the balance with {amount} GEL on: {DateTime.Now}. User's current balance is {user.Balance} GEL");
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        static void Withdraw(AtmUser user)
        {
            Console.Write("Enter the amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0 && amount <= user.Balance)
            {
                user.Balance -= amount;
                Console.WriteLine($"Withdrawn {amount} GEL. Your current balance: {user.Balance} GEL");
                LogOperation($"User named {user.Name} {user.LastName} cashed out for {amount} GEL on: {DateTime.Now}. User's current balance is {user.Balance} GEL");
            }
            else if (amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
            else
            {
                Console.WriteLine("Withdrawal amount exceeds your current balance.");
            }
        }

        static void ViewHistory()
        {
            Console.WriteLine("Transaction History:");

            List<string> logEntries = File.ReadAllLines(logFilePath).ToList();

            foreach (var entry in logEntries)
            {
                Console.WriteLine(entry);
            }
        }

        static void LogOperation(string logMessage)
        {
            File.AppendAllLines(logFilePath, new[] { logMessage });
        }

        static void LoadUsers()
        {
            if (File.Exists(usersFilePath))
            {
                string json = File.ReadAllText(usersFilePath);
                users = JsonSerializer.Deserialize<List<AtmUser>>(json);
            }
        }

        static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usersFilePath, json);
        }

        static bool IsPersonalNumberUnique(string personalNumber)
        {
            return users.All(u => u.PersonalNumber != personalNumber);
        }

        static AtmUser FindUser(string personalNumber, string password)
        {
            return users.FirstOrDefault(u => u.PersonalNumber == personalNumber && u.Password == password);
        }
    }
}

