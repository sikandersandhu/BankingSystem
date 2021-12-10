using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace BankingSystem
{   
    /// <summary>
/// The bank consits a list of accounts, transactions, subscriptions, account numbers and methods with
/// relevant functionality for the respective lists.
/// The overriden ExecuteTransaction method, calls methods from the DepositTransaction,
/// WithdrawTransaction and TransferTransaction classes to execute transaction.
/// The overriden RollbackTransaction method, calls methods from the DepositTransaction,
/// WithdrawTransaction and TransferTransaction classes to rollback transaction.
/// Also provides a method to print transaction history
/// </summary>
    [DataContract]
    [KnownType(typeof(List<Account>))]
    [KnownType(typeof(List<Transaction>))]
    [KnownType(typeof(List<AccountHolder>))]
    [KnownType(typeof(List<long>))]
    class Bank : IPublisher, IListAccess
    {
        [DataMember(Name ="List_of_subscribers")]
        public List<AccountHolder> Subscribers { get; private set; }

        public Transaction Transaction { get; private set; }

        public Account Account { get; private set; }
        public Account ToAccount { get; private set; }

        [DataMember(Name = "List_of_accounts")]
        public List<Account> Accounts { get; private set; }

        [DataMember(Name = "List_of_transactions")]
        private List<Transaction> _transactions;

        /// <summary>
        /// List of exisiting account numbers to ensure there are no duplicates
        /// </summary>
        [DataMember(Name = "List_of_account_numbers")]
        public List<long> AccNumbers { get; private set; }

        // contrcutor: initializes list of accounts, transactions, account numbers and subscribers
        public Bank()
        {
            Accounts = new List<Account>();
            _transactions = new List<Transaction>();
            AccNumbers = new List<long>();
            Subscribers = new List<AccountHolder>();
        }

        /// <summary>
        /// Generates random account numbers and adds a unique number 
        /// to the list of account numbers
        /// </summary>
        /// <returns>a unique 8 to 9 digit account number</returns>
        public long GenAccNum()
        {
            long accNum;
            // Generates and saves eight digit random numeric account number
            accNum = (DateTime.Now.Ticks / 10) % 1000000000;

            // If list is empty, adds account number to list
            if (AccNumbers.Count == 0) AccNumbers.Add(accNum);
            else
            {
                // while list of account numbers already contains the
                // account number continues to generate new numbers
                // till a unique account number is found
                while (AccNumbers.Contains(accNum))
                {
                    accNum = (DateTime.Now.Ticks / 10) % 1000000000;
                }
                // and adds the number to the list
                AccNumbers.Add(accNum);
            }
            // returns new accout number for the account
            return accNum;
        }
        /// <summary>
        /// removes account number from the list of account numbers
        /// </summary>
        /// <param name="accNum">the account number to be removed</param>
        public void RemAccNum(long accNum)
        {
            if (AccNumbers == null) throw new NullReferenceException("\n***** NullReference: Account numbers list not found *****\n");
            if (!AccNumbers.Contains(accNum)) throw new InvalidOperationException("\n***** InvalidOperation: Account number not found. *****\n");

            AccNumbers.Remove(accNum);
            Console.WriteLine("\n***** Success: Account number removed *****\n");
        }

        // IListAccess interface methods to access the list of accounts
        public void Add<T>(T toAdd)
        {
            if (toAdd == null) throw new NullReferenceException("\n***** NullReference: Reference for the account is missing*****\n");
            if (Accounts == null) throw new NullReferenceException("\n***** NullReference: Accounts list not found *****\n");
            if (Accounts.Contains(toAdd as Account)) throw new InvalidOperationException("\n***** InvalidOperation: An account with the given details already exists *****\n");

            Accounts.Add(toAdd as Account);
            Console.WriteLine("\n***** Success: {0} added *****\n", toAdd.GetType().Name.ToString());
        }
        public void Rem<T>(T toRem)
        {
            if (toRem == null) throw new NullReferenceException("\n***** NullReference: Reference for the account is missing*****\n");
            if (Accounts == null) throw new NullReferenceException("\n***** NullReference: Accounts list not found *****\n");
            if (!Accounts.Contains(toRem as Account)) throw new InvalidOperationException("\n***** InvalidOperation: An account with the given details does not exists *****\n");

            Accounts.Remove(toRem as Account);
            Console.WriteLine("\n***** Success: {0} removed *****\n", toRem.GetType().Name);
        }
        public T Find<T>(params object[] list) where T: class
        {
            var accNum = list[0];
            foreach (object obj in list) if (obj == null) throw new ArgumentException("\n***** ArgumnetException: argument can not be null *****\n");
            if (Accounts == null) throw new NullReferenceException("\n***** NullReference: Accounts list empty *****\n");

            Account toFind = null;

            foreach (Account acc in Accounts) if (acc.AccNum == (long)accNum) toFind = acc;

            return toFind as T;
        }

        // Execute and Rollback transaction functions that call polymorphically call
        // DepositTransaction, WithdrawTransaction and TransferTransaction methods depending
        // on the the type of transaction object provided as the parameter.
        public void ExecuteTransaction(Transaction transaction)
        {
            // Execute the transaction
            transaction.Execute();

            Transaction = transaction;
            Account = FindTransacAcc(Transaction, out Account toAcc);
            ToAccount = toAcc;
            this.Notify();

            // Adds transactions to the list of transactions
            _transactions.Add(transaction);        
        }
        public void RollbackTransaction(Transaction transaction)
        {
            // Execute the transaction
            transaction.Rollback();

            Transaction = transaction;
            Account = FindTransacAcc(Transaction, out Account toAcc);
            ToAccount = toAcc;

            this.Notify();

            // Adds transactions to the list of transactions
            _transactions.Add(transaction);          
        }

        /// <summary>
        /// Prints transaction history
        /// </summary>
        public void PrintTransactionHistory()
        {
            if (_transactions != null)
            {
                // Prints transactions starting from the latest.
                for (int i = _transactions.Count - 1; i >= 0; i--) _transactions[i].Print();
            }
            else Console.WriteLine("There are currently no transactions to print.");
        }

        // IPublisher interface Attach, Detach, Notify methods and two other methods to 
        // add and remove subscribers from the list and notify them of the transaction they are observing.
        public static Account FindTransacAcc(Transaction transaction, out Account toAcc)
        {
            toAcc = null;
            Account acc = null;

            // Variables to save account type
            Type type = transaction.GetType();         // What type of transaction is it?

            if (type == typeof(DepositTransaction))    // if it is DepositTransaction
            {
               acc  = (transaction as DepositTransaction).Account; // assign account ref
            }
            if (type == typeof(WithdrawTransaction))    // if it is WithdrawTransaction
            {
                acc = (transaction as WithdrawTransaction).Account; // assign account ref
            }
            if (type == typeof(TransferTransaction))    // if it is TransferTransaction
            {
                acc = (transaction as TransferTransaction).FromAccount; // assign account ref
                toAcc = (transaction as TransferTransaction).ToAccount; // assign account ref
            }
            
            return acc; // Return current account ref
        }
        public void Attach(AccountHolder observer)
        {
            if (observer == null) throw new NullReferenceException("\n***** Fail: NullReference - Subscription information is missing *****\n");
            if (Subscribers == null) throw new NullReferenceException("\n***** NullReference: Subscribers list not found *****\n");
            if (Subscribers.Contains(observer)) throw new InvalidOperationException("\n***** Fail: A subscription already exists for the account holder *****\n");
            
            Subscribers.Add(observer);
            Console.WriteLine("\n***** Success: Subscriber added *****\n");
        }
        // Detaches observers from the subscribers list
        public void Detach(AccountHolder observer)
        {
            if (observer == null) throw new NullReferenceException("\n***** Fail: NullReference - Subscription information is missing *****\n");
            if (Subscribers == null) throw new NullReferenceException("\n***** NullReference: Subscribers list not found *****\n");
            if (!Subscribers.Contains(observer)) throw new InvalidOperationException("\n***** InvalidOperation: There are no current subscriptions for the account holder. *****\n");
            
            Subscribers.Remove(observer);
            Console.WriteLine("\n***** Success: subscriber removed *****\n");
        }
        public AccountHolder FindObserver(long accNum, string fName, string lName)
        {
            if ( fName == null || lName == null ) throw new NullReferenceException("\n***** Infomation missing: Both first and last names are required. *****\n");

            AccountHolder _ = null;

            if (Subscribers.Count > 0)
            {
                foreach (AccountHolder subscriber in Subscribers)
                {
                    if (subscriber.AccNum == accNum && subscriber.FName == fName && subscriber.LName == lName) _ = subscriber;
                    else throw new InvalidOperationException("\n***** A subscription with the provided details could not be found *****\n");
                }                   
            }
            return _;
        }
        public AccountHolder FindObserver(long accNum)
        {
            AccountHolder _ = null;

            if (Subscribers.Count > 0)
            {
                foreach (AccountHolder subscriber in Subscribers)
                {
                    if (subscriber.AccNum == accNum) _ = subscriber;
                    else throw new InvalidOperationException("\n***** A subscription with the provided details could not be found *****\n");
                }
            }
            return _;
        }

        // Notifies all observers about the event of interest
        // as it occurs.
        public void Notify()
        {
            foreach (AccountHolder accHolder in Subscribers) 
                if (accHolder.AccNum == Account.AccNum)
                {
                    Console.WriteLine("\n***** Subject: Notifying subscribers *****\n");
                    accHolder.Update(this);
                    if (ToAccount != null)
                        if (accHolder.AccNum == ToAccount.AccNum) accHolder.Update(this);
                }
        }
    }
}
