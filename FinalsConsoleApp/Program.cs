using System;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


namespace FinalsConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // CalculatorLoop();
            // GuesserLoop();
            // Hangman();
             ATMSimulator();
        }

        #region HangmanFunctions
        static void Hangman()
        {
            List<string> words = new List<string>
               {
                   "apple", "banana", "orange", "grape", "kiwi",
                   "strawberry", "pineapple", "blueberry", "peach", "watermelon"
               };
            string hangmanAns;
            string[] blankAns;
            ansGenerator(words, out hangmanAns, out blankAns);
            ansChecker(blankAns, hangmanAns);
        }

        static void ansGenerator(List<string> words, out string hangmanAns, out string[] blankAns)
        {
            Random rnd = new Random();
            int rndIndex = rnd.Next(0, words.Count);
            hangmanAns = words[rndIndex];
            Console.WriteLine("Keyword is set, you have 6 tries to reveal the answer!");
            blankAns = new string[hangmanAns.Length];
            for (int i = 0; i < hangmanAns.Length; i++)
            {
                blankAns[i] = "_";
            }
        }
        static void ansChecker(string[] blankAns, string hangmanAns)
        {
            for (int i = 6; i >= 1; i--)
            {
                bool letterFound = false;
                bool rightAnswerDisplayed = false;
                Console.WriteLine("");
                for (int k = 0; k < blankAns.Length; k++)
                {
                    Console.Write(blankAns[k] + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("Input a letter, or a final guess: ");
                string input = Console.ReadLine();
                if (input.Length == 0)
                {
                    Console.Write("Please Provide a letter or a final guess in order to continue: ");
                    i++;
                }
                else if (input.Length > 1)
                {
                    string finalAns = input;
                    if (finalAns == hangmanAns)
                    {
                        Console.WriteLine("That's correct! You win");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong answer! You lost!");
                        break;
                    }
                }
                else if (input.Length == 1)
                {
                    string inputChar = input;
                    for (int j = 0; j < hangmanAns.Length; j++)
                    {
                        if (inputChar == hangmanAns[j].ToString())
                        {
                            letterFound = true;
                            if (!rightAnswerDisplayed)
                            {
                                Console.WriteLine("Right answer! Tries left: " + (i - 1));
                                rightAnswerDisplayed = true;
                            }
                            blankAns[j] = hangmanAns[j].ToString();
                        }
                    }
                    if (!letterFound && !rightAnswerDisplayed)
                    {
                        Console.WriteLine("Wrong answer! Tries left: " + (i - 1));
                    }
                }

                if (string.Join("", blankAns) == hangmanAns)
                {
                    Console.WriteLine("Congratulations! You guessed the word!");
                    break;
                }
                if (i == 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Sorry, you've used up all the tries. The correct answer was: " + hangmanAns);
                    break;
                }
            }
        }
        #endregion HangmanFunctions
        #region numGuesserFunctions
        static void Hinter(int input, int ans)
        {
            if (input < ans)
            {
                Console.WriteLine("Higher!");
            }
            else if (input > ans)
            {
                Console.WriteLine("Lower!");
            }
            else
            {
                Console.WriteLine("Your guess is right! Great job");
            }
        }

        static void GuesserLoop()
        {
            int answ = ModeSelector();
            int input;
            Console.WriteLine("Number is set, try to guess it! you have 10 tries");
            for (int i = 1; i <= 10; i++)
            {
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    Hinter(input, answ);
                    if (input == answ) break;
                }
                else
                {
                    Console.WriteLine("Please provide only an integer as a guess");
                    i--;
                }
                if (i == 10) Console.WriteLine("You've used up all of the tries");

            }
        }

        static int ModeSelector()
        {
            Random random = new Random();
            int ans = 0;
            Console.WriteLine("Please choose the difficulty mode: ");
            string mode = Console.ReadLine();
            if (mode == "Easy")
            {
                ans = random.Next(1, 25);
            }
            else if (mode == "Medium")
            {
                ans = random.Next(1, 50);
            }
            else if (mode == "Hard")
            {
                ans = random.Next(1, 100);
            }
            return ans;
        }
        #endregion numGuesserFunctions
        #region calcfunctions
        static void CalculatorLoop()
        {
            int x = 0, y = 0;
            string myOperator = "";

            while (true)
            {
                Console.WriteLine("Enter the numbers, then an operator: ");
                if (int.TryParse(Console.ReadLine(), out x) && int.TryParse(Console.ReadLine(), out y))
                {
                    myOperator = Console.ReadLine();
                    DoCalculation(myOperator, x, y);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter valid integers.");
                }
            }
        }

        static void DoCalculation(string myOperator, int x, int y)
        {
            int ans = 0;
            try
            {
                switch (myOperator)
                {
                    case "+":
                        ans = x + y;
                        break;
                    case "-":
                        ans = x - y;
                        break;
                    case "*":
                        ans = x * y;
                        break;
                    case "/":
                        if (y != 0)
                        {
                            ans = x / y;
                        }
                        else
                        {
                            throw new DivideByZeroException("Enter a divisor which is not 0");
                        }
                        break;
                    default:
                        Console.WriteLine("Please provide a correct operator");
                        return;
                }
                Console.WriteLine("Answer: " + ans);
            }
            catch (DivideByZeroException dv) { Console.WriteLine(dv.Message); return; }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }

        }
        #endregion calcfunctions
        #region ATM
        static void ATMSimulator() {
            ATM atm = new ATM();
            atm.AtmMenu();
        }
        #endregion ATM
    }
}
       

