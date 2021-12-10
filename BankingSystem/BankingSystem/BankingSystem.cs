using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace BankingSystem_Iteration5_AddAccountTypes
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
            // Creates the  bank
            Bank bank = new();

            // Deserialises transactions history and account details
            try
            {
                Deserialize(out bank, @"BankData");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Displays bank menu
            ExecuteBankMenu(bank);
        }
        public static void ExecuteBankMenu(Bank bank)
        {

            // Create menu options
            BankMenuOption menuOptions;

            // Repeat menu options
            do
            {
                // Prompt user to choose menu and validate
                menuOptions = BankMenu();

                // Menu options
                switch (menuOptions - 1)
                {
                    case BankMenuOption.CREATE_NEW_ACCOUNT:
                        ExecuteAccountMenu(bank);
                        break;
                    case BankMenuOption.DEPOSIT_MONEY:
                        MakeDeposit<SavingsAccount>(bank);
                        break;
                    case BankMenuOption.WITHDRAW_MONEY:
                        MakeWithdrawal<SavingsAccount>(bank);
                        break;
                    case BankMenuOption.TRANSFER_MONEY:
                        MakeTransfer(bank);
                        break;
                    case BankMenuOption.CREDIT_ACCOUNT_MAKE_PURCHASE:
                        MakeWithdrawal<CreditAccount>(bank);
                        break;
                    case BankMenuOption.CREDIT_ACCOUNT_MAKE_PAYMENT:
                        MakeDeposit<CreditAccount>(bank);
                        break;
                    case BankMenuOption.TERM_DEPOSIT_CALCULATE_INTEREST:
                        Account _ = CalculateInterest(bank, out decimal interest, out int days);
                        Console.WriteLine("\t***** Calculate interest *****\n\nInterest on {0} days is: {1:C}", days, interest);                        
                        break;
                    case BankMenuOption.TERM_DEPOSIT_ADD_INTEREST:
                        DepositInterest(bank);
                        break;
                    case BankMenuOption.PRINT_SUMMARY:
                        PrintSummary(bank);
                        break;
                    case BankMenuOption.PRINT_TRANSACTION_HISTORY:
                        PrintTransactionHistory(bank);
                        break;
                    case BankMenuOption.QUIT:
                        Console.WriteLine("\t***** My Bank *****\n");
                        Console.WriteLine("Thanks for banking with us. Good day.");
                        try
                        {
                            Serialize(bank);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                }
            } while (menuOptions - 1 != BankMenuOption.QUIT);
        }
        public static void ExecuteAccountMenu(Bank bank)
        {
            // Menu options type variable
            AccountMenuOption userSelection;
            do
            {
                // Read user menu selection
                userSelection = AccountMenu();

                switch (userSelection)
                {
                    case AccountMenuOption.CREDIT_ACCOUNT:
                        CreateCreditAccount(bank);
                        break;
                    case AccountMenuOption.SAVINGS_ACCOUNT:
                        CreateSavingsAccount(bank);
                        break;
                    case AccountMenuOption.TERM_DEPOSIT_ACCOUNT:
                        CreateTermDepositAccount(bank);
                        break;
                    case AccountMenuOption.QUIT:
                        Console.WriteLine("Thank you for choosing your new account with us!");
                        break;
                }
            } while (userSelection != AccountMenuOption.QUIT);           
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
        public static BankMenuOption BankMenu()
        {
            // Prompt user to select menu option and Validate input
            string userInput = PromptUserInput("1.  Create new account \n2.  Desposit money \n3.  Withdraw money \n4.  Transfer money \n5.  Credit account: Make purchase \n" +
                "6.  Credit account: Make payment \n7.  Term deposit account: Calculate interest \n8.  Term Deposit Account: Add interest \n9.  Print account summary \n10. Print transaction history \n11. Quit", "\n\t***** Banking Menu *****");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 11);

            // Users menu selection
            BankMenuOption userSelection = (BankMenuOption)validatedInput;

            // return users selection
            return userSelection;
        }
        public static AccountMenuOption AccountMenu()
        {
            // Prompt user to select menu option and Validate input
            string userInput = PromptUserInput("\n\n\t***** Account Menu *****\n\n1. Create Credit Account \n2. Create Savings Account \n3. Create term deposit account \n4. Quit\n");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 4);

            // Users menu selection
            AccountMenuOption userSelection = (AccountMenuOption)validatedInput;

            // return users selection
            return userSelection;
        }
        /// <summary>
        /// Prompts user for account name and initial account balance, reads and
        /// validates user input. Creates a new account and adds the account to the bank
        /// </summary>
        /// <param name="bank">The bank to add the account to</param>
        /// <exception cref="InvalidOperationException">if the account already exists</exception>
        public static void CreateSavingsAccount(Bank bank)
        {
            // Regex account name validation expression
            string regexExpression = @"^([A-Z]?[[a-z]{1,20})$|^([A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$|^([A-Z]?[[a-z]{1,20}\040[A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$";
            // Error message, if the user input does not match the expression
            string errorMessage = "Invalid input. Maximum three words. No numeric or symbolic characters allowed.";

            // Prompts user for account name and validates
            string accountName = Validator.RegexValidation(PromptUserInput("Enter account name: "), regexExpression, errorMessage);

            // Prompts user for Initial acount balance and validates
            decimal amount = Validator.ValidateNumeric<decimal>(PromptUserInput("Enter initial balance: "));

            // Creates a new account
            Account savingsAccount = new SavingsAccount(amount, accountName);

            // Tries to add account into the bank
            try
            {
                bank.AddAccount(savingsAccount);
                Console.WriteLine("\n\t***** Account Created *****\n", accountName);
                Console.WriteLine(savingsAccount.ToString());
            }

            // If the account already exists displays the caught exception
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }
        public static void CreateCreditAccount(Bank bank)
        { 
            // Regex account name validation expression
            string regexExpression = @"^([A-Z]?[[a-z]{1,20})$|^([A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$|^([A-Z]?[[a-z]{1,20}\040[A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$";
            // Error message, if the user input does not match the expression
            string errorMessage = "Invalid input. Maximum three words. No numeric or symbolic characters allowed.";

            // Prompts user for account name and validates
            string accountName = Validator.RegexValidation(PromptUserInput("Enter account name: "), regexExpression, errorMessage);

            // Prompts user for Initial acount balance and validates
            decimal amount = Validator.ValidateNumeric<decimal>(PromptUserInput("Enter initial balance: "));

            // Prompts user for credit limit and validate
            int limit = Validator.ValidateNumeric<int>(PromptUserInput("Enter credit limit: "));

            // Creates a new account
            Account creditAccount = new CreditAccount(amount, accountName, limit);

            // Tries to add account into the bank
            try
            {
                bank.AddAccount(creditAccount);
                Console.WriteLine("\n\t***** Account Created *****\n");
                Console.WriteLine(creditAccount.ToString());
            }

            // If the account already exists displays the caught exception
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }
        public static void CreateTermDepositAccount(Bank bank)
        {
            // Regex account name validation expression
            string regexExpression = @"^([A-Z]?[[a-z]{1,20})$|^([A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$|^([A-Z]?[[a-z]{1,20}\040[A-Z]?[a-z]{1,20}\040[A-Z]?[a-z]{1,20})$";
            // Error message, if the user input does not match the expression
            string errorMessage = "Invalid input. Maximum three words. No numeric or symbolic characters allowed.";

            // Prompts user for account name and validates
            string accountName = Validator.RegexValidation(PromptUserInput("Enter account name: "), regexExpression, errorMessage);

            // Prompts user for Initial acount balance and validates
            decimal amount = Validator.ValidateNumeric<decimal>(PromptUserInput("Enter initial balance: "));

            // Prompts user for interest rate and validate
            decimal interestRate = Validator.ValidateNumeric<decimal>(PromptUserInput("Enter interest rate: "));

            // Prompts user for term deposit duration and validate
            int duration = Validator.ValidateNumeric<int>(PromptUserInput("Number of days for term deposit: "));

            // Creates a new account
            Account termDepositAccount = new TermDepositAccount(amount, accountName, interestRate, duration);

            // Tries to add account into the bank
            try
            {
                bank.AddAccount(termDepositAccount);
                Console.WriteLine("\n\t***** Account Created *****\n", accountName);
                Console.WriteLine(termDepositAccount.ToString());
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

            // Find if the account exists
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
        public static void MakeDeposit<T>(Bank bank)
        {
            try
            {
                decimal amount = 0;

                // Account validation

                // Checks if the account exists
                Account account = FindAccount(bank);
                // If account with the provided name does not exist, throws exception
                if (account == null) throw new Exception("***** Account does not exist *****");

                // Checks what type of account is requesting deposit 
                if (account.GetType() == typeof(T))
                {
                    // Transaction title
                    if (account.GetType() == typeof(CreditAccount)) Console.WriteLine("\t\n***** Make payment *****\n");
                    else Console.WriteLine("\t\n***** Deposit Money *****\n");

                    // Prompts user for amount and validate

                    // If SavingsAccount, user prompt is:
                    string userPrompt = "Enter Deposit amount: ";
                    // if CreditAccount, user prompt is:
                    if (account.GetType() == typeof(CreditAccount)) userPrompt = "Enter Payment amount: ";

                    amount = Validator.ValidateNumeric<decimal>(PromptUserInput(userPrompt));

                    try
                    {
                        // Initializes transaction for deposit
                        Transaction deposit = new DepositTransaction(account, amount);

                        // Makes deposit
                        try
                        {
                            bank.ExecuteTransaction(deposit);
                            deposit.Print();
                        }

                        // If it fails shows caught exception
                        catch (InvalidOperationException ioe)
                        {
                            deposit.Print();
                            Console.WriteLine(ioe.Message);
                        }
                    }
                    catch (NullReferenceException nre)
                    {
                        Console.WriteLine(nre.Message);
                    }
                }
                else Console.WriteLine("\n***** Invalid operation: This account is not a {0} *****\n", typeof(T).Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
        public static void MakeWithdrawal<T>(Bank bank)
        {
            try
            {
                // Account validation
                Account account = FindAccount(bank);

                // If account with the provided name does not exist, throws exception
                if (account == null) throw new Exception("***** Account does not exist *****");
                if (account.GetType() == typeof(T))
                {
                    // Transaction title
                    if (account.GetType() == typeof(CreditAccount)) Console.WriteLine("\t\n***** Make purchase *****\n");
                    else Console.WriteLine("\t\n***** Withdraw Money *****\n");

                    // If SavingsAccount, user prompt is:
                    string userPrompt = "Enter Withdrawal amount: ";
                    // if CreditAccount, user prompt is:
                    if (account.GetType() == typeof(CreditAccount)) userPrompt = "Enter Purchase amount: ";

                    // Prompt user for withdrawal amount and validate
                    decimal amount = Validator.ValidateNumeric<decimal>(PromptUserInput(userPrompt));

                    try
                    {
                        // Initializes transaction for withdrawal
                        Transaction withdraw = new WithdrawTransaction(account, amount);

                        // Makes withdrawal
                        try
                        {
                            bank.ExecuteTransaction(withdraw);
                            withdraw.Print();
                        }

                        // if it fails shows caught exception
                        catch (InvalidOperationException ioe)
                        {
                            withdraw.Print();
                            Console.WriteLine(ioe.Message);
                        }
                    }
                    catch (NullReferenceException nre)
                    {
                        Console.WriteLine(nre.Message);
                    }
                }
                else Console.WriteLine("***** Invalid operation: This account is not a {0} *****", typeof(T).Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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

            // Check for any uncaught exceptions
            try
            {
                Account fromAccount, toAccount;

                // Checks if the from and to accounts exist
                fromAccount = FindAccount(bank);
                // If account with the provided name does not exist, throws exception
                if (fromAccount == null) throw new Exception("***** From account does not exist *****");

                // if user tries to transfer funds from credit or term deposit account
                // an error message is displayed and methods terminates
                if (fromAccount.GetType() == typeof(CreditAccount) || fromAccount.GetType() == typeof(TermDepositAccount))
                {
                    Console.WriteLine("Invalid operation: Can not transfer money from {0}", fromAccount.GetType().Name);
                    return;
                }

                toAccount = FindAccount(bank);
                // If account with the provided name does not exist, throws exception
                if (toAccount == null) throw new Exception("***** Transfer failed. To account does not exist *****");

                // Prompt user for transfer amount and validate
                string userInputAmount = PromptUserInput("Enter transfer amount: ");
                decimal amount = Validator.ValidateNumeric<decimal>(userInputAmount);

                try
                {
                    // Initializes the transaction for transfer
                    Transaction transfer = new TransferTransaction(fromAccount, toAccount, amount);

                    // Makes transfer
                    try
                    {
                        bank.ExecuteTransaction(transfer);
                        transfer.Print();
                    }

                    // If it fails displays the exception caught
                    catch (InvalidOperationException ioe)
                    {
                        if (transfer.Executed && !transfer.Success) bank.RollbackTransaction(transfer);

                        transfer.Print();
                        Console.WriteLine(ioe.Message);
                    }
                }
                catch (NullReferenceException nre)
                {
                    Console.WriteLine(nre.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occured: {0}\nInner exception: {1}\nMessage: {2}", e.GetType().ToString(), e.InnerException.ToString(), e.Message);
            }
        }
        public static Account CalculateInterest(Bank bank, out decimal interest, out int days)
        {
            // Checks if the account exist
            Account account = FindAccount(bank);
            interest = 0;
            days = 0;

            // if the account is TermDepositAccount
            if (account.GetType() == typeof(TermDepositAccount))
            {
                try
                {
                    // calculates the interest for the user specified no. of days
                    TermDepositAccount tDepAccount = (account as TermDepositAccount);
                    days = Validator.ValidateNumeric<int>(PromptUserInput("Enter number of days: "), 0, int.MaxValue);
                    interest = tDepAccount.CalculateInterest(days);
                }
                catch(Exception e) 
                {
                    Console.WriteLine(e.Message);
                }
            }
            // if the account is not TermDespoistAccount Type, notifies the user of the error.
            else Console.WriteLine("***** Invalid operation: The account is not a Term Deposit Account *****");

            return account;
        }
        public static void DepositInterest(Bank bank)
        {
            Console.WriteLine("\t***** Deposit interest *****\n\n");
            Account account = CalculateInterest(bank, out decimal interest, out _);

            Transaction deposit = new DepositTransaction(account, interest);

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
    public enum BankMenuOption { CREATE_NEW_ACCOUNT, DEPOSIT_MONEY, WITHDRAW_MONEY, TRANSFER_MONEY, CREDIT_ACCOUNT_MAKE_PURCHASE, CREDIT_ACCOUNT_MAKE_PAYMENT, TERM_DEPOSIT_CALCULATE_INTEREST, TERM_DEPOSIT_ADD_INTEREST, PRINT_SUMMARY, PRINT_TRANSACTION_HISTORY, QUIT };
    public enum AccountMenuOption { CREDIT_ACCOUNT = 1, SAVINGS_ACCOUNT = 2, TERM_DEPOSIT_ACCOUNT = 3, QUIT = 4}
}
