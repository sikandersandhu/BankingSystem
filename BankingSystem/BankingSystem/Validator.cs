using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BankingSystem_Iteration4_serializing
{
    /// <summary>
    /// Static class for validator methods
    /// </summary>
    static class Validator
    {
        /// <summary>
        /// This generic validator, validates int and decimal amounts.
        /// </summary>
        /// <typeparam name="T">int or decimal</typeparam>
        /// <param name="userInput">input to be validated</param>
        /// <returns>Returns a validated and parsed decimal or int value.</returns>
        public static T ValidateNumeric<T>(string userInput)
        {
            // If vlaue is decimal
            Type type = typeof(T);
            if(type == typeof(decimal))
            {
                // While the user input is not an integer or decimal value,
                // continues to prompt user for desired input.
                while (decimal.TryParse(userInput, out _) != true)
                {
                    Console.WriteLine("Invalid input. The amount should be an integer or decimal number.");
                    userInput = Console.ReadLine();
                }
            }

            // If value is int
            if(type == typeof(int))
            {
                // While the user input is not an integer or decimal value,
                // continues to prompt user for desired input.
                while (int.TryParse(userInput, out _) != true)
                {
                    Console.WriteLine("Invalid input. The amount should be an integer or decimal number.");
                    userInput = Console.ReadLine();
                }
            }
            
            // Returns the validated int or decimal amount
            return (T) Convert.ChangeType(userInput, typeof(T));           
        }

        /// <summary>
        /// Generic validator, validates int or decimal amount within a range
        /// </summary>
        /// <typeparam name="T">int or decimal</typeparam>
        /// <param name="userInput">user input to be validated</param>
        /// <param name="min">minimum range</param>
        /// <param name="max">maximum range</param>
        /// <returns>a validated int or decimal amount that is within range</returns>
        public static T ValidateNumeric<T>(string userInput, int min, int max)where T: IComparable
        {
            // Validates int or decimal amount and stores input
            T input = ValidateNumeric<T>(userInput);

            // if the input is int
            Type type = typeof(T);
            if(type == typeof(int))
            {
                // While input is less than min and more than max,
                // keeps prompting user for a valid input within the range
                while ((input.CompareTo(min) < 0) && input.CompareTo(max) > 0)
                {
                    Console.WriteLine("The value must be greater than {0} and less than {1}", min, max);
                    input = ValidateNumeric<T>(Console.ReadLine());
                }
            } 
            
            // returns validated input within the range
            return input;
        }

        /// <summary>
        /// Regex user input validator
        /// </summary>
        /// <param name="userInput">User input to be validated</param>
        /// <param name="regexExpression">The regular expression which the regex engine will use 
        /// to match user input to perform data validation.</param>
        /// <param name="errorMessage">The error message to be displayed if the user input could not be matched succesfully</param>
        /// <returns>the validated user input</returns>
        /// <exception cref="RegexParseException"> if the regular expression can not be parsed.</exception>
        /// <exception cref="RegexMatchTimeoutException">if the operation times out based on the Time span set in the regex options </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string RegexValidation(string userInput, string regexExpression, string errorMessage)
        {
            // Regex variable reference for data validation
            Regex myRegex;
            // Match variable reference to store the result of a match
            Match match;

            // Input validation
            try
            {
                // Initializes the regex engine with the regex expression, with a timeout limit.
                myRegex = new Regex(regexExpression, RegexOptions.None, TimeSpan.FromSeconds(1));
                // Matches the user input with regex expression.
                match = myRegex.Match(userInput);

                // If not matched, repeats till a successful match is found
                while (!match.Success)
                {
                    // error message
                    Console.WriteLine(errorMessage);
                    // read input again
                    userInput = Console.ReadLine();
                    // match it again
                    match = myRegex.Match(userInput);
                }
            }

            // If validation fails, catches and displays relevant exception.
            catch (RegexParseException rpe)
            {
                Console.WriteLine("Regex parse exception. Could not validate user input: {0}", userInput);
                Console.WriteLine(rpe.Message);
            }
            catch (RegexMatchTimeoutException rmte)
            {
                Console.WriteLine("The operation times out after {0:N0} miliseconds", rmte.MatchTimeout.TotalMilliseconds);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine(aoore.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }

            // If validation succeeds, returns validated input
            return userInput;
        }
    }
}
