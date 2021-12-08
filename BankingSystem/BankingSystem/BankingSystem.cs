using System;
using System.Text.RegularExpressions;

namespace BankingSystem_Iteration2
{
    /// <summary>
    /// This public enumerated type <c>Menu</c> represent menu options
    /// <para>used in the switch case statements to provide menu options functionality.</para>
    /// <see cref="switch"/>
    /// </summary>
    public enum MenuOptions { CREATE_NEW_ACCOUNT, DEPOSIT_MONEY, WITHDRAW_MONEY, TRANSFER_MONEY, PRINT_SUMMARY, QUIT };

    /// <summary>
    /// Tester class for banking system
    /// </summary>
    class BankingSystem
    {
        /// <summary>
        /// Displays a message to the user, reads user input and return it.
        /// </summary>
        /// <param name="message">Message to prompt user for input</param>
        /// <param name="heading">Optional heading string</param>
        /// <returns></returns>
        public static string PromptUserInput(string message, string heading = null)
        {
            if (heading != null)
            {
                Console.WriteLine(heading);
                Console.WriteLine();
            }           
            Console.WriteLine(message);
            return (Console.ReadLine());
        }

        /// <summary>
        /// Displays a list of menu options, reads and validates user input
        /// </summary>
        /// <returns>returns a validated enum <c>MenuOptions</c> value. 
        /// <remarks>this value is used by the switch case options for menu functionality</remarks>
        /// </returns>
        public static MenuOptions ReadUserInput()
        {
            // Prompt user to select menu option and Validate input
            string userInput = PromptUserInput("1. Create new account \n2. Desposit money \n3. Withdraw money \n4. Transfer money \n5. Print account summary \n6. Quit", "\n\t***** Banking Menu *****");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 6);

            // Users menu selection
            MenuOptions menuSelection = (MenuOptions)validatedInput;

            // return users selection
            return menuSelection;
        }

        /// <summary>
        /// Prompts user for account name and initial account balance, reads and
        /// validates user input. Creates a new account and adds the account to the bank
        /// </summary>
        /// <param name="bank">The bank to add the account to</param>
        /// <exception cref="InvalidOperationException">if the account already exists</exception>
        public static void CreateAccount(Bank bank)
        {
            // Regex account name validation expression
            string regexExpression = @"^([A-Z]?[[a-z]{1,20})$|^([A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$|^([A-Z]?[[a-z]{1,20}\040[A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$";
            // Error message, if the user input does not match the expression
            string errorMessage = "Invalid input. Maximum three words. No numeric or symbolic characters allowed.";

            // Prompts user for account name and validates
            string  userInput = PromptUserInput("Enter account name: ");
            string accountName = Validator.RegexValidation(userInput, regexExpression, errorMessage);

            // Prompts user for Initial acount balance and validates
            string userInputAmount = PromptUserInput("Enter initial balance: ");
            decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

            // Creates a new account
            Account account = new Account(amount, accountName);

            // Tries to add account into the bank
            try
            {
                bank.AddAccount(account);
                Console.WriteLine("Account name: {0} \n\t***** Account Succesfully Created *****\n", accountName);
            }

            // If the account already exists displays the caught exception
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }

        /// <summary>
        /// Prompts user for account name, searches for the account in the
        /// bank and returns the search result.
        /// </summary>
        /// <param name="bank">bank in which to search the account</param>
        /// <returns>either return the account address or null</returns>
        private static Account FindAccount(Bank bank)
        {
            // Prompt user for account name
            string accountName = PromptUserInput("Enter account name: ");

            // FInd if the account exists
            Account account = bank.GetAccount(accountName);

            // Return the result of the search
            return account;
        }

        /// <summary>
        /// Validates account and deposit amount.
        /// If the account exists, initializes a deposit transaction 
        /// and tries to make the deposit. 
        /// If the account does not exist or the deposit fails, shows 
        /// relevant error messages.
        /// </summary>
        /// <param name="bank">The bank which has the account in which
        /// the deposit will be made</param>
        /// <exception cref="InvalidOperationException">if the deposit fails.</exception>
        public static void MakeDeposit(Bank bank)
        {
            Console.WriteLine("\t***** Deposit Money *****");

            // Check if the account exists
            Account account = FindAccount(bank);

            // If the account does not exist
            if (account == null) Console.WriteLine("\n***** Deposit could not be processed. Account does not exist  *****\n");  
            
            // else if the account exists
            else
            {
                // Prompt user for deposit amount and validate
                string userInputAmount = PromptUserInput("Enter deposit amount: ");
                decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

                // Initialize transaction for deposit
                DepositTransaction deposit = new DepositTransaction(account, amount);

                // Try to make the deposit
                try
                {
                    bank.ExecuteTransaction(deposit);
                }

                // If it fails show error message
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }
            }         
        }

        /// <summary>
        /// Validates account and withdrawal amount.
        /// If the account exists, initializes a withdrawal transaction 
        /// and tries to make the deposit. 
        /// If the account does not exist or the withdrawal fails, shows 
        /// relevant error messages.
        /// </summary>
        /// <param name="bank">The bank which has the account from which the 
        /// withdrawal will be made</param>
        /// <exception cref="InvalidOperationException">if the withdrawal fails.</exception>
        public static void MakeWithdrawal(Bank bank)
        {
            Console.WriteLine("\t***** Withdraw Money *****");

            // Check if the account exist
            Account account = FindAccount(bank);

            // If account does not exist
            if (account == null) Console.WriteLine("\n***** Withdrawal could not be processed. Withdrawal account does not exist  *****\n");

            // else if account exists
            else
            {
                // Prompt user for withdrawal amount and validate
                string userInputAmount = PromptUserInput("Enter withdrawal amount: ");
                decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

                // Initialize transaction for withdrawal
                WithdrawTransaction withdraw = new WithdrawTransaction(account, amount);

                // try to make withdrawal
                try
                {
                    bank.ExecuteTransaction(withdraw);
                }

                // if it fails show error message
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }
            }
        }

        /// <summary>Transfers money between two accounts</summary>
        /// <para>
        /// Validates from account, to account and withdrawal amount.
        /// If the accounts exists, initializes a transfer transaction 
        /// and tries to make the transfer. 
        /// If any of the accounts do not exist or the transfer fails, shows 
        /// relevant error messages.
        /// </para>
        /// <param name="bank">The bank which has the accounts between which the 
        /// transfer will take place</param>
        /// <exception cref="InvalidOperationException">if the transfer fails.</exception>
        public static void MakeTransfer(Bank bank)
        {

            Console.WriteLine("\t***** Transfer Money *****");
      
            Account fromAccount, toAccount;

            // Check if the from and to accounts exist
            fromAccount = FindAccount(bank);
            toAccount = FindAccount(bank);

            // If any of the accounts or both the accounts do not exist,
            // display the relevant error message
            if (fromAccount == null || toAccount == null)
            {
                if (fromAccount == null) Console.WriteLine("***** Transfer not possible. Account named '{0}' does not exist *****", fromAccount.Name);
                if (toAccount == null) Console.WriteLine("***** Transfer not possible. Account named '{0}' does not exist *****", toAccount.Name);
                if (fromAccount == null && toAccount == null) Console.WriteLine("***** Transfer not possible. Accounts named '{0}' and '{1}' do not exist *****", fromAccount.Name, toAccount.Name);
            }

            // Else if both accounts exist 
            else
            {
                // Prompt user for transfer amount and validate
                string userInputAmount = PromptUserInput("Enter transfer amount: ");
                decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

                // Initialize the transaction for transfer
                TransferTransaction transfer = new TransferTransaction(fromAccount, toAccount, amount);

                // Try to make the transfer
                try
                {
                    bank.ExecuteTransaction(transfer);
                }

                // If it fails display relevant error message
                catch (InvalidOperationException ioe)
                { 
                    Console.WriteLine(ioe.Message);
                }
            }  
        }

        /// <summary>
        /// prompts user for account name. If the account exists displays 
        /// the account summary, else shows error message
        /// </summary>
        /// <param name="bank">the bank which has the account that the user 
        /// wants the account summary for</param>
        public static void PrintSummary(Bank bank)
        {
            // Check if the account exists
            Account account = FindAccount(bank);

            // If the account does not exist
            if (account == null) Console.WriteLine("\n***** The account does not exist *****\n"); 

            // Else if the account exists
            else
            {
                Console.WriteLine("\t***** My Bank *****\n");

                // Print account summary
                Console.WriteLine(account.ToString());
            }
        }

        /// <summary>
        /// Entry point of the Banking system
        /// </summary>
        static void Main(string[] args)
        {
            // Create bank
            Bank bank = new Bank();

            // Create menu options
            MenuOptions menuOptions;

            // Repeat menu options
            do
            { 
                // Prompt user to choose menu and validate
                menuOptions = ReadUserInput();

                // Menu options
                switch (menuOptions-1)
                {
                    case MenuOptions.CREATE_NEW_ACCOUNT:
                        CreateAccount(bank);
                        break;

                    case MenuOptions.DEPOSIT_MONEY:
                        MakeDeposit(bank);
                        break;

                    case MenuOptions.WITHDRAW_MONEY:
                        MakeWithdrawal(bank);
                        break;

                    case MenuOptions.TRANSFER_MONEY:
                        MakeTransfer(bank);
                        break;
                       
                    case MenuOptions.PRINT_SUMMARY:
                        PrintSummary(bank);
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
