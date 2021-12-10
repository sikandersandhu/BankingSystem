using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace BankingSystem
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
            /*try
            {
                Deserialize(out bank, @"BankData");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/
			
            // Displays bank menu
            ExecuteBankMenu(bank);
        }

        // Displays bank menu options and returns user selection
        public static BankMenuOption BankMenu()
        {
            // Prompt user to select menu option and Validate input
            string userInput = Validator.PromptUserInput("1.  Create new account \n2.  Delete account \n\n3.  Add account holder \n4.  Remove account holder \n\n5.  Subscribe: Account notifications \n6.  Unsubscribe: Account notifications \n\n7.  Desposit money \n8.  Withdraw money \n9.  Transfer money \n\n10. Credit account: Make purchase \n" +
                "11. Credit account: Make payment \n\n12. Term deposit account: Calculate interest \n13. Term Deposit Account: Add interest \n\n14. Print account summary \n15. Print transaction history \n\n16. Quit", "\n\t***** Banking Menu *****");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 16);

            // Users menu selection
            BankMenuOption userSelection = (BankMenuOption)validatedInput;

            // return users selection
            return userSelection;
        }
        // Displays account menu options and returns user selection
        public static AccountMenuOption AccountMenu()
        {
            // Prompt user to select menu option and Validate input
            string userInput = Validator.PromptUserInput("\n\n\t***** Account Menu *****\n\n1. Create Credit Account \n2. Create Savings Account \n3. Create term deposit account \n4. Quit\n");
            int validatedInput = Validator.ValidateNumeric<int>(userInput, 1, 4);

            // Users menu selection
            AccountMenuOption userSelection = (AccountMenuOption)validatedInput;

            // return users selection
            return userSelection;
        }

        // Bank: Executes user selection by calling relevant static method
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
                    case BankMenuOption.REMOVE_ACCOUNT:
                        RemAccount(bank);
                        break;
                    case BankMenuOption.ADD_ACCOUNT_HOLDER:
                        AddAccountHolder(bank);
                        break;
                    case BankMenuOption.REMOVE_ACCOUNT_HOLDER:
                        RemAccountHolder(bank);
                        break;
                    case BankMenuOption.ADD_SUBSCRIBER:
                        AddSubscriber(bank);
                        break;
                    case BankMenuOption.REMOVE_SUBSCRIBER:
                        RemSubscriber(bank);
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
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                }
            } while (menuOptions - 1 != BankMenuOption.QUIT);
        }
        // Account: Executes user selection by calling relevant static method
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

        // Creates savings account, account holder and adds account into the bank.
        public static void CreateSavingsAccount(Bank bank)
        {
            // Prompts user for Initial acount balance and validates
            decimal amount = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput("Enter initial balance: "), 0.0M, decimal.MaxValue);

            // Generates new account number
            long accNum = bank.GenAccNum();

            // Creates a new account
            Account savingsAccount = new SavingsAccount(amount, accNum);

            try
            {
                // adds account to the bank
                bank.Add<Account>(savingsAccount);
                Console.WriteLine("\n***** Add account holder *****\n");
                // Creates account holder
                AccountHolder accHolder = CreateAccountHolder(accNum);
                // Adds account holder to the account
                AddAccountHolder(savingsAccount, accHolder);
                Console.WriteLine("\n***** Account Created *****\n");
                Console.WriteLine(savingsAccount.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Creates credit account, account holder and adds account into the bank.
        public static void CreateCreditAccount(Bank bank)
        { 
            // Prompts user for Initial acount balance and validates
            decimal balance = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput("Enter initial balance: "), 0.0M, decimal.MaxValue);

            // Prompts user for credit limit and validate
            int limit = Validator.ValidateNumeric<int>(Validator.PromptUserInput("Enter credit limit: "), 0, int.MaxValue);

            // Generates new account number
            long accNum = bank.GenAccNum();

            // Creates a new account
            Account creditAccount = new CreditAccount(balance, accNum, limit);

            try
            {
                // Add account to the list of accounts in the bank
                bank.Add<Account>(creditAccount);

                Console.WriteLine("\n***** Add account holder *****\n");

                // Creates a new account holder
                AccountHolder accHolder = CreateAccountHolder(accNum);
                // Adds account holder to the account
                AddAccountHolder(creditAccount, accHolder);
                Console.WriteLine("\n***** Account Created *****\n");
                Console.WriteLine(creditAccount.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Creates term deposit account, account holder and adds account into the bank.
        public static void CreateTermDepositAccount(Bank bank)
        {
            // Prompts user for Initial acount balance and validates
            decimal amount = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput("Enter initial balance: "), 0.0M, decimal.MaxValue);

            // Prompts user for interest rate and validate
            decimal interestRate = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput("Enter interest rate: "), 0.0M, 20.0M);

            // Prompts user for term deposit duration and validate
            int duration = Validator.ValidateNumeric<int>(Validator.PromptUserInput("Number of days for term deposit: "), 0, int.MaxValue);

            // Generates new account number
            long accNum = bank.GenAccNum();

            // Creates a new account
            Account termDepositAccount = new TermDepositAccount(amount, accNum, interestRate, duration);

            try
            {
                // Adds the account to the list of account holders in the bank
                bank.Add<Account>(termDepositAccount);

                Console.WriteLine("\n***** Add account holder *****\n");

                // Create an account holder
                AccountHolder accHolder = CreateAccountHolder(accNum);
                // Adds the account holder to the account
                AddAccountHolder(termDepositAccount, accHolder);
                Console.WriteLine("\n***** Account Created *****\n");
                Console.WriteLine(termDepositAccount.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Removes account from the list of accounts in the bank
        public static void RemAccount(Bank bank)
        {
            // prompts user for account number and validates
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));

            try
            {
                // finds if the account exists in the bank
                Account acc = bank.Find<Account>(accNum);
                if (acc == null) Console.WriteLine("\n***** NullReference: An account with the provided account number does not exist *****\n");
                // if account exists, remove account
                else
                {
                    // find if any of the account holders have subscribed for notifications
                    AccountHolder subscriber = bank.FindObserver(accNum);
                    // If subscription exists, detach subscriber
                    if (subscriber != null) bank.Detach(subscriber);
                    // remove account from the bank
                    bank.Rem<Account>(acc);
                    // remove account number from the list of existing account numbers
                    bank.RemAccNum(accNum);
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Creates and return the reference of a new account holder
        public static AccountHolder CreateAccountHolder(long accNum)
        {
            // Error message, if the user input does not match the regex
            string errorMessage = "Invalid input:\n\nOnly one word allowed (do not enter any empty spaces)\nOnly first letter can be capital (optional)\nNumeric and symbolic characters not allowed\nMaximum 30 letters";
            // regex to validate name
            string regexName = @"^([A-Z]?[[a-z]{1,30})$";

            // Prompts user for account name and validates
            string fName = Validator.RegexValidation(Validator.PromptUserInput("Enter first name: "), regexName, errorMessage);
            string lName = Validator.RegexValidation(Validator.PromptUserInput("Enter last name: "), regexName, errorMessage);

            AccountHolder accHolder = new (fName, lName, accNum);

            return accHolder;
        }
        // Bank menu option: Adds a new account holder to the account.
        public static void AddAccountHolder(Bank bank)
        {
            // Error message, if the user input does not match the regex
            string errorMessage = "Invalid input:\n\nOnly one word allowed (do not enter any empty spaces)\nOnly first letter can be capital (optional)\nNumeric and symbolic characters not allowed\nMaximum 30 letters";
            // regex to validate name
            string regexName = @"^([A-Z]?[[a-z]{1,30})$";

            // prompts user for account number and validates
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));
          
            try
            {
                // finds if the account exists in the bank
                Account acc = bank.Find<Account>(accNum);
                if (acc == null) throw new NullReferenceException("\n***** NullReference: An account with the given account number could not be found *****\n");
                else
                {
                    // Prompts user for account name and validates
                    string fName = Validator.RegexValidation(Validator.PromptUserInput("Enter first name: "), regexName, errorMessage);
                    string lName = Validator.RegexValidation(Validator.PromptUserInput("Enter last name: "), regexName, errorMessage);

                    // finds if the account holder exists in the account
                    AccountHolder accHolder = acc.Find<AccountHolder>(accNum, fName, lName);
                    if (accHolder == null)
                    {
                        // create an account holder instance
                        accHolder = new AccountHolder(fName, lName, accNum);
                        // adds account holder to the account and prints confirmation. 
                        acc.Add<AccountHolder>(accHolder);
                    }
                    else throw new InvalidOperationException("\n***** InvalidOperation: An Account holder with the provided details already exists *****\n");
                }          
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch(InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Adds account holder to the account at the time of new account creation
        public static void AddAccountHolder(Account acc, AccountHolder accHolder)
        {
            try
            {
                // adds account holder to the account holder list in the account and prints confirmation. 
                acc.Add<AccountHolder>(accHolder);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch(InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Bank menu option: Removes an account holder from the account.
        public static void RemAccountHolder(Bank bank)
        {
            // Error message, if the user input does not match the regex
            string errorMessage = "Invalid input:\n\nOnly one word allowed (do not enter any empty spaces)\nOnly first letter can be capital (optional)\nNumeric and symbolic characters not allowed\nMaximum 30 letters";
            // regex to validate name
            string regexName = @"^([A-Z]?[[a-z]{1,30})$";

            // prompts user for account number and validates
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));

            try
            {
                // finds if the account exists in the bank
                Account acc = bank.Find<Account>(accNum);
                if (acc == null) Console.WriteLine("\n***** NullReference: An account with the provided account number does not exist *****\n");
                else
                {
                    // Prompts user for account name and validates
                    string fName = Validator.RegexValidation(Validator.PromptUserInput("Enter first name: "), regexName, errorMessage);
                    string lName = Validator.RegexValidation(Validator.PromptUserInput("Enter last name: "), regexName, errorMessage);
                    // finds if the account holder exists in the account
                    AccountHolder accHolder = acc.Find<AccountHolder>(accNum, fName, lName);
                    // adds account holder to the account and prints confirmation. 
                    if (accHolder != null) acc.Rem<AccountHolder>(accHolder);
                    else throw new NullReferenceException("\n***** NullReference: An account holder with the provided details could not be found *****\n");
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Removes an account holder from the account at the time of removing an account.
        public static void RemAccountHolder(Account acc, AccountHolder accHolder)
        {
            try
            {
                // removes account holder from the accound holders list in the account and prints confirmation. 
                acc.Rem<AccountHolder>(accHolder);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Bank menu option: Adds subscriber to the subscribers list for transaction notifications
        public static void AddSubscriber(Bank bank)
        {
            // Error message, if the user input does not match the regex
            string errorMessage = "Invalid input:\n\nOnly one word allowed (do not enter any empty spaces)\nOnly first letter can be capital (optional)\nNumeric and symbolic characters not allowed\nMaximum 30 letters";
            // regex to validate name
            string regexName = @"^([A-Z]?[[a-z]{1,30})$";

            // prompts user for account number and validates
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));

            try
            {
                // finds if the account exists in the bank
                Account acc = bank.Find<Account>(accNum);
                if (acc == null) throw new NullReferenceException("\n***** NullReference: An account with the given account number could not be found *****\n");
                else
                {
                    // Prompts user for account name and validates
                    string fName = Validator.RegexValidation(Validator.PromptUserInput("Enter first name: "), regexName, errorMessage);
                    string lName = Validator.RegexValidation(Validator.PromptUserInput("Enter last name: "), regexName, errorMessage);

                    // finds if the account holder exists in the account
                    AccountHolder accHolder = acc.Find<AccountHolder>(accNum, fName, lName);
                    // if account holder exists, adds account holder as subscriber in the subscribers list in the bank
                    if (accHolder != null) bank.Attach(accHolder);
                    else throw new NullReferenceException("\n***** NullReference: An AccountHoler with the provided details could not be found *****\n");
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Adds subscriber to the subscribers list during the creation of a new account
        public static void AddSubscriber(Bank bank, AccountHolder subscriber)
        {
            try
            {
                // adds subscriber to the subscribers list in the bank and prints confirmation. 
                bank.Attach(subscriber);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Bank menu option: Removes a subscriber from the subscribers list
        public static void RemSubscriber(Bank bank)
        {
            // Error message, if the user input does not match the regex
            string errorMessage = "Invalid input:\n\nOnly one word allowed (do not enter any empty spaces)\nOnly first letter can be capital (optional)\nNumeric and symbolic characters not allowed\nMaximum 30 letters";
            // regex to validate name
            string regexName = @"^([A-Z]?[[a-z]{1,30})$";

            // prompts user for account number and validates
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));

            try
            {
                // finds if the account exists in the bank
                Account acc = bank.Find<Account>(accNum);
                if (acc == null) Console.WriteLine("\n***** NullReference: An account with the provided account number does not exist *****\n");
                else
                {
                    // Prompts user for account name and validates
                    string fName = Validator.RegexValidation(Validator.PromptUserInput("Enter first name: "), regexName, errorMessage);
                    string lName = Validator.RegexValidation(Validator.PromptUserInput("Enter last name: "), regexName, errorMessage);
                    // finds if the account holder exists in the account
                    AccountHolder accHolder = acc.Find<AccountHolder>(accNum, fName, lName);
                    // if account holder exists, remove account holder from the subscribers list in the bank and print confirmation
                    if (accHolder != null) bank.Detach(accHolder);
                    else throw new NullReferenceException("\n***** NullReference: An account holder with the provided details could not be found *****\n");
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Removes a subscriber from the subscribers list at the time of removing an account from the bank.
        public static void RemSubscriber(Bank bank, AccountHolder subscriber)
        {
            try
            {
                // remvoes account holder from the subscriber list in the bank and prints confirmation. 
                bank.Detach(subscriber);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

                // Prompt user for account name
                long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));
                // Checks if the account exists
                Account account = bank.Find<Account>(accNum);
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

                    amount = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput(userPrompt));

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
        /// and tries to make the withdrawal. 
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
                // Prompt user for account name
                long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));

                // Account validation
                Account account = bank.Find<Account>(accNum);

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
                    decimal amount = Validator.ValidateNumeric<decimal>(Validator.PromptUserInput(userPrompt));

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
                // Prompt user for account name
                long fromAccNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter from account number: "));
                // Checks if the from and to accounts exist
                fromAccount = bank.Find<Account>(fromAccNum);
                // If account with the provided name does not exist, throws exception
                if (fromAccount == null) throw new Exception("***** From account does not exist *****");

                // if user tries to transfer funds from credit or term deposit account
                // an error message is displayed and methods terminates
                if (fromAccount.GetType() == typeof(CreditAccount) || fromAccount.GetType() == typeof(TermDepositAccount))
                {
                    Console.WriteLine("Invalid operation: Can not transfer money from {0}", fromAccount.GetType().Name);
                    return;
                }
                // Prompt user for account name
                long toAccNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter to account number: "));
                toAccount = bank.Find<Account>(toAccNum);
                // If account with the provided name does not exist, throws exception
                if (toAccount == null) throw new Exception("***** Transfer failed. To account does not exist *****");

                // Prompt user for transfer amount and validate
                string userInputAmount = Validator.PromptUserInput("Enter transfer amount: ");
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

        // Calculates interest based on interst rate and number of days provides
        public static Account CalculateInterest(Bank bank, out decimal interest, out int days)
        {
            // Prompt user for account name
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));
            // Checks if the account exist
            Account account = bank.Find<Account>(accNum);
            interest = 0;
            days = 0;

            // if the account is TermDepositAccount
            if (account.GetType() == typeof(TermDepositAccount))
            {
                try
                {
                    // calculates the interest for the user specified no. of days
                    TermDepositAccount tDepAccount = (account as TermDepositAccount);
                    days = Validator.ValidateNumeric<int>(Validator.PromptUserInput("Enter number of days: "), 0, int.MaxValue);
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
        // Calculates interest based on provided interest and number of days and
        // deposits into the term deposit account.
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

        // Prints summary for an account associated with the provided account number
        public static void PrintSummary(Bank bank)
        {
            // Prompt user for account name
            long accNum = Validator.ValidateNumeric<long>(Validator.PromptUserInput("Enter account number: "));
            // Check if the account exists
            Account account = bank.Find<Account>(accNum);

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

        // Prints history of all past transactions
        public static void PrintTransactionHistory(Bank bank)
        {
            bank.PrintTransactionHistory();
        }

        // Serializes Bank into a local data base in XML format
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
        // Deserializes from the local database 
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
    /// This public enumerated type <c>BankMenuOptions</c> represent menu options
    /// <para>used in the switch case statements to provide menu options functionality.</para>
    /// <see cref="switch"/>
    /// </summary>
    public enum BankMenuOption { CREATE_NEW_ACCOUNT, REMOVE_ACCOUNT, ADD_ACCOUNT_HOLDER, REMOVE_ACCOUNT_HOLDER, ADD_SUBSCRIBER, REMOVE_SUBSCRIBER, DEPOSIT_MONEY, WITHDRAW_MONEY, TRANSFER_MONEY, CREDIT_ACCOUNT_MAKE_PURCHASE, CREDIT_ACCOUNT_MAKE_PAYMENT, TERM_DEPOSIT_CALCULATE_INTEREST, TERM_DEPOSIT_ADD_INTEREST, PRINT_SUMMARY, PRINT_TRANSACTION_HISTORY, QUIT };
    
    /// <summary>
    /// This public enumerated type <c>AccountMenuOptions</c> represent menu options
    /// <para>used in the switch case statements to provide menu options functionality.</para>
    /// <see cref="switch"/>
    /// </summary>
    public enum AccountMenuOption { CREDIT_ACCOUNT = 1, SAVINGS_ACCOUNT = 2, TERM_DEPOSIT_ACCOUNT = 3, QUIT = 4}
}
