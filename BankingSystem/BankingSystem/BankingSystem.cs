using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace BankingSystem_Iteration4_serializing
{
    /// <summary>
    /// Tester class for banking system
    /// </summary>
    class BankingSystem
    {    
        
        /// <summary>
        /// Entry point of the Banking system
        /// </summary>
        static void Main(string[] args)
        {
            // Create bank
            Bank bank = new();
            try
            {
                Deserialize(out bank, @"BankData");
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occured: {0}\nMore details: {1}\nMessage: {2}", e.GetType().ToString(), e.InnerException.ToString(), e.Message);
            }

            // Display bank menu
            BankMenu(bank);
        }

        public static void BankMenu(Bank bank)
        {

            // Create menu options
            MenuOptions menuOptions;

            // Repeat menu options
            do
            {
                // Prompt user to choose menu and validate
                menuOptions = ReadMenuSelection();

                // Menu options
                switch (menuOptions - 1)
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

                    case MenuOptions.PRINT_TRANSACTION_HISTORY:
                        PrintTransactionHistory(bank);
                        break;

                    case MenuOptions.QUIT:
                        Console.WriteLine("\t***** My Bank *****\n");
                        Console.WriteLine("Thanks for banking with us. Good day.");
                        try
                        {
                            Serialize(bank);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("The following error occured: {0}\nMore details: {1}\nMessage: {2}", e.GetType().ToString(), e.InnerException.ToString(), e.Message);
                        }
                        break;
                }
            } while (menuOptions - 1 != MenuOptions.QUIT);
        }

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
        public static MenuOptions ReadMenuSelection()
        {
            // Prompt user to select menu option and Validate input
            string userInput = PromptUserInput("1. Create new account \n2. Desposit money \n3. Withdraw money \n4. Transfer money \n5. Print account summary \n6. Print transaction history \n7. Quit", "\n\t***** Banking Menu *****");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 7);

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
            string userInput = PromptUserInput("Enter account name: ");
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
                Console.WriteLine("\n\t***** Account Created *****\n", accountName);
                Console.WriteLine(account.ToString());
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

            // Prompt user for deposit amount and validate
            string userInputAmount = PromptUserInput("Enter deposit amount: ");
            decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

            try
            {
                // Initialize transaction for deposit
                Transaction deposit = new DepositTransaction(account, amount);

                // Try to make the deposit
                try
                {
                    bank.ExecuteTransaction(deposit);
                    deposit.Print();
                }

                // If it fails show error message
                catch (InvalidOperationException ioe)
                {
                    deposit.Print();
                    Console.WriteLine(ioe.Message);
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("\n***** Deposit failed. Account does not exist  *****\n");
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

            // Prompt user for withdrawal amount and validate
            string userInputAmount = PromptUserInput("Enter withdrawal amount: ");
            decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

            try
            {
                // Initialize transaction for withdrawal
                Transaction withdraw = new WithdrawTransaction(account, amount);

                // try to make withdrawal
                try
                {
                    bank.ExecuteTransaction(withdraw);
                    withdraw.Print();
                }

                // if it fails show error message
                catch (InvalidOperationException ioe)
                {
                    withdraw.Print();
                    Console.WriteLine(ioe.Message);
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("\n***** Withdrawal failed. Account does not exist  *****\n");
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

            // Prompt user for transfer amount and validate
            string userInputAmount = PromptUserInput("Enter transfer amount: ");
            decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

            try
            {
                // Initialize the transaction for transfer
                Transaction transfer = new TransferTransaction(fromAccount, toAccount, amount);

                // Try to make the transfer
                try
                {
                    bank.ExecuteTransaction(transfer);
                    transfer.Print();
                }

                // If it fails display relevant error message
                catch (InvalidOperationException ioe)
                {
                    if (transfer.Executed && !transfer.Success) bank.RollbackTransaction(transfer);

                    transfer.Print();
                    Console.WriteLine(ioe.Message);
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Transfer failed. One or more accounts you entered, do not exist.");
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

        public static void PrintTransactionHistory(Bank bank)
        {
            bank.PrintTransactionHistory();
        }

        public static void Serialize(Bank bank)
        {
            // Creates a new DataContractSerializer to serialize a stream
            DataContractSerializer s = new (typeof(Bank));

            // FileStream to write a stream
            //FileStream writer = File.Open(@"BankData", FileMode.Create, FileAccess.Write);

            var settings = new XmlWriterSettings() { Indent = true };
            using (var writter = XmlWriter.Create(@"BankData", settings))

                // Serializes the Bank Lists to xml doc
                s.WriteObject(writter, bank);

            //Closes resources
            //writer.Close();
        }
        public static void Deserialize(out Bank bank, string fileName)
        {
            // Creates a new DataContractSerializer to serialize a stream
            DataContractSerializer s = new(typeof(Bank));

            // Opens the file to read
            FileStream fStream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            // Reads from the file
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fStream, new XmlDictionaryReaderQuotas());

            // Deserializes xml doc to a new instance of bank
            bank = (Bank)s.ReadObject(reader, true);

            reader.Close();
            fStream.Close();
        }
    }

    /// <summary>
    /// This public enumerated type <c>Menu</c> represent menu options
    /// <para>used in the switch case statements to provide menu options functionality.</para>
    /// <see cref="switch"/>
    /// </summary>
    public enum MenuOptions { CREATE_NEW_ACCOUNT, DEPOSIT_MONEY, WITHDRAW_MONEY, TRANSFER_MONEY, PRINT_SUMMARY, PRINT_TRANSACTION_HISTORY, QUIT };
}
