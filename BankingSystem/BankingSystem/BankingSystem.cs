using System;
using System.Text.RegularExpressions;

namespace BankingSystem_1
{
    /// <summary>
    /// This public enumerated type <c>Menu</c> represent menu options
    /// <para>used in the switch case statements to provide menu options functionality.</para>
    /// <see cref="switch"/>
    /// </summary>
    public enum MenuOptions { DEPOSIT_MONEY, WITHDRAW_MONEY, PRINT_SUMMARY, QUIT };

    /// <summary>
    /// Tester class for banking system
    /// </summary>
    class BankingSystem
    {
        /// <summary>
        /// This static function displays a message on the console, prompting the user for input.
        /// The <c>userInput</c> is then compared with the supplied <paramref name="regexExpression"/>
        /// using regex engine to validate user input.
        /// <para>
        /// If the user input is valid, the function returns validated<c>userInput</c>.
        /// If the user input is not valid an <paramref name="errorMessage"/> is 
        /// displayed on the console with an example of what is expectedand 
        /// the user is prompted to provide input again in the desired format.
        /// </para>
        /// </summary>
        /// <param name="promptUser">Message to be displayed on console to prompt user input.</param>
        /// <param name="regexExpression">The regular expression which the regex engine will use 
        /// to match user input to perform data validation.</param>
        /// <param name="errorMessage">The error message to be displayed if the user input could not be matched succesfully</param>
        /// <returns>the validated user input</returns>
        /// <exception cref="RegexParseException"> if the regular expression can not be parsed.</exception>
        /// <exception cref="RegexMatchTimeoutException">if the operation times out based on the Time span set in the regex options </exception>
        private static string RegexValidation(string promptUser, string regexExpression, string errorMessage)
        {
            /****** Variable declarations *******/
            // Regex variable for data validation
            Regex myRegex;
            // Match variable to match regex expression
            Match match;
            // User input to validate
            string userInput;

            // Console message and read user input
            Console.WriteLine(promptUser);
            userInput = Console.ReadLine();

            // Input validation
            try
            {
                myRegex = new Regex(regexExpression, RegexOptions.None, TimeSpan.FromSeconds(1));
                match = myRegex.Match(userInput);
                while (!match.Success)
                {
                    Console.WriteLine(errorMessage);
                    userInput = Console.ReadLine();
                    match = myRegex.Match(userInput);
                }
            }
            catch(RegexParseException rpe)
            {
                Console.WriteLine("Regex parse exception. Could not validate user input: {0}", userInput);
                Console.WriteLine(rpe.Message);
            }
            catch (RegexMatchTimeoutException rmte)
            {
                Console.WriteLine("The operation times out after {0:N0} miliseconds", rmte.MatchTimeout.TotalMilliseconds);
            }

            return userInput;
        }

        /// <summary>
        /// This method prompts user for input, validates the input and returns the validated decimal input.
        /// <para>While the user input is not a decimal or integer value, the user receives an error message 
        /// and is prompted to keep trying till the correct input is received.</para>
        /// </summary>
        /// <param name="promptUser">The message to be displayed on the console to prompt user input.</param>
        /// <returns>Returns a validated and parsed decimal user input value.</returns>
        public static decimal ValidateAmount(string promptUser)
        {
            string userInput;
            
            // Prompt user for input, read and store user input in userInput variable.
            Console.WriteLine(promptUser);
            userInput = Console.ReadLine();

            // While the user input is not an integer or decimal value,
            // continue to prompt user for desired input.
            while (decimal.TryParse(userInput, out _) != true)
            {
                Console.WriteLine("Invalid input. The amount should be an integer or decimal number.");
                userInput = Console.ReadLine();
            }

            // Parse validated string input to decimal and return
            return decimal.Parse(userInput);
            
        }

        /// <summary>
        /// This function displays a list of menu options, reads and validates user input
        /// </summary>
        /// <returns>returns a validated enum <c>MenuOptions</c> value. 
        /// <remarks>this value is used by the switch case options for menu functionality</remarks>
        /// </returns>
        public static MenuOptions ReadUserInput()
        {
                // Prompt user to select menu option and Validate input
                string validatedInput = RegexValidation("\n\t\tBanking Menu - Enter menu number for selection\n\t1. Desposit money\n\t2. Withdraw money \n\t3.Print account summary \n\t4.Quit", @"^[1-4]$", "Invalid input. Enter a menu option from 1 - 4.");

                // Switch options to manipulate user account
                MenuOptions menuSelection = (MenuOptions)int.Parse(validatedInput);

                return menuSelection;
        }

        /// <summary>
        /// This method prompts the user to enter a deposit amount, validates user input and calls the <c>Deposit</c>
        /// method from the <c>Account</c> class to initiate the deposit.
        /// <para>If succesfull, a success message is displayed. 
        /// If unsuccesfull, an exception is displayed and caught. </para>
        /// </summary>
        /// <param name="account">the object used to call the <c>Deposit</c> method to deposit funds into the account.</param>
        /// <exception cref="InvalidOperationException">If the deposit amount is not greater than zero an exception is 
        /// thrown, which is caught in the catch block of this method.
        /// <para>User receives the message "Deposit failed. The amount must be greater than zero". </para>
        public static void MakeDeposit(Account account)
        {
            Console.WriteLine("\t***** Deposit Money *****\n");

            // Validate deposit amount
            decimal validatedInput = ValidateAmount("Enter deposit amount: ");

            // Call deposit method from Account class to initiate deposit
            // if succesfull provide success message
            // if unsuccesfull throw exception and catch 
            try
            {
                bool success = account.Deposit(validatedInput);

                if (success) Console.WriteLine("***** Deposit successful *****");

                else throw new InvalidOperationException("Deposit failed. Deposit amount must be greater than zero.");
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }

        /// <summary>
        /// This method prompts user to enter a withdrawal amount, validates user input and calls the <c>Withdraw/c>
        /// method from the <c>Account</c> class to initiate withdrawal.
        /// <para>If the withrawal is successful, a success message is displayed, otherwise exceptions with relevant 
        /// messages are thrown and caught.</para>
        /// </summary>
        /// <param name="account">the object used to call the <c>Withdraw</c> method to withdraw funds from the account</param>
        /// <exception cref="InvalidOperationException">If the deposit amount is less than zero or greater than account balance, 
        /// the <c>MakeDeposit</c> method throws an exception that is caught in the catch block.
        /// <para>if amount is not greater than zero: User receives the message: 
        /// Withdrawal failed. Withdrawal amount must be greater than zero."
        /// if the amount is greater than the account balalnce, user receives the message:
        /// "Withdrawal failed. Insufficient funds in the account. Try a different amount."
        /// User is then prompted to make another menu selections</para>
        public static void MakeWithdrawal(Account account)
        {
            Console.WriteLine("\t***** Withdraw Money *****\n");

            // Validate deposit amount
            decimal validatedAmount = ValidateAmount("Enter withdrawal amount: ");

            // Initiate withdrawal:
            // if succesfull, provide success message
            // if unsuccesfull, throw exception and catch
            try
            {
                bool success = account.Withdraw(validatedAmount);

                if (success) Console.WriteLine("***** Withdrawal successful *****");

                else
                {
                    if (validatedAmount <= 0) throw new InvalidOperationException("Withdrawal failed. Withdrawal amount must be greater than zero.");
                    else throw new InvalidOperationException("Withdrawal failed. Insufficient funds in the account. Try a different amount.");
                }               
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }

        /// <summary>
        /// This method calls the <c>TOString()</c> method from the <c>Account</c> class to print users Account summary.
        /// </summary>
        /// <param name="account">the object used to call the <c>ToString</c> method fro mthe <c>Account</c> class</param>
        public static void PrintSummary(Account account)
        {
            Console.WriteLine("\t***** My Bank *****\n");

            Console.WriteLine(account.ToString());
        }

        /// <summary>
        /// This is the entry point of the Banking system Testing
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            MenuOptions menuOptions;

            //Prompt user for account name and data validation
            string validatedInput = RegexValidation("Enter account name: ", @"^([A-Z]?[[a-z]{1,20})$|^([A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$|^([A-Z]?[[a-z]{1,20}\040[A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$", "Invalid input. Maximum three words. No numeric or symbolic characters allowed.");

            // Account instance
            Account account = new Account(0, validatedInput);

            // Bank menu options
            do
            { 
                menuOptions = ReadUserInput();

                switch (menuOptions-1)
                {
                    case MenuOptions.DEPOSIT_MONEY:
                        MakeDeposit(account);
                        break;

                    case MenuOptions.WITHDRAW_MONEY:
                        MakeWithdrawal(account);
                        break;
                       
                    case MenuOptions.PRINT_SUMMARY:
                        PrintSummary(account);
                        break;

                    case MenuOptions.QUIT:
                        Console.WriteLine("\t***** My Bank *****\n");
                        Console.WriteLine("Thanks for banking with us. Good day.");
                        break;
                }
            } while (menuOptions-1 != MenuOptions.QUIT);
        }
    }
}
