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
    /// The methods in this class merely call methods from the account class to 
    /// carry out operations on the account.
    /// The methods also provide status messages and throw relevant exceptions to the user.
    /// These methods perform the following operations:
    /// - Deposit money by calling Deposit() 
    /// - Rollback deposit by calling Withdraw()
    /// - Print account summary by calling ToString()
    /// </summary>
    [DataContract]
    class DepositTransaction: Transaction
    {
        /// <summary>
        /// This field will be initialized with the account in which the 
        /// deposit is to be made
        /// </summary>
        [DataMember(Name = "Current_Account_Summary", Order = 2)]
        public Account Account { get; private set; }

        /// <summary>
        /// Stores the account balance post transaction
        /// Initialized in the constructor.
        /// </summary>
        [DataMember(Name = "Post_Transaction_Balance", Order = 1)]
        private readonly decimal _postTransactionBalance;

        /// <summary>
        /// Public property to read the success status of the deposit
        /// </summary>
        [DataMember]
        public override bool Success
        {
            get { return _success; }
        }

        // constructor: initializes the account to withdraw from, the amount
        // and sets the post transaction readonly amount field
        public DepositTransaction(Account account, decimal amount) : base(amount)
        {
            Account = account;
            _postTransactionBalance = Account.Balance + _amount;
        }

        /// <summary>
        /// Calls the deposit method to make a deposit into the account.
        /// Prints the status messages and summaries and throws relevant 
        /// error messages if the deposit fails.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exceptions is thrown if the deposit failed.</exception>
        public override void Execute()
        { 
            // If the deposit has not been executed
            if (!Executed)
            {
                base.Execute();
                // executes the deposit and store the status               
                bool succeeded = Account.Deposit(_amount);

                // If deposit succeeded
                if (succeeded)
                {
                    // changes executed and success status to true
                    // and prints account summary                   
                    _success = true;
                }
                // If deposit failed
                else throw new InvalidOperationException("\n***** Amount must be greater than zero. *****\n");
            }
            // if the deposit has already been executed,
            // throws the following error message
            else throw new InvalidOperationException("\n***** The transaction has already been succesfully executed. *****\n");
        }

        /// <summary>
        /// Calls the withdraw method to Rolls back a deposit from
        /// the account.Prints the status, and account summary 
        /// and throws relevant exceptions.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exception is thrown if the reversal fails</exception>
        /// <exception cref="InvalidOperationException">This exception is thrown if the reversal has 
        /// already been succuesfully executed.</exception>
        public override void Rollback()
        {
            // If the depsoit has not been reversed
            if (!Reversed)
            {
                base.Rollback();

                //Reverses deposit and stores the status
                bool succeeded = Account.Withdraw(_amount);

                // If not succeeded
                if (!succeeded) throw new InvalidOperationException("Rollback failed. Not enough funds in the acccount.");
            }
            // If the deposit has already been reversed, thorws the following exception
            else throw new InvalidOperationException("The amount has already been succesfully rolledback.");
        }

        /// <summary>
        /// Prints the success status, date stamp of the deposit the account summary
        /// </summary>
        public override void Print()
        {
            if (Success)
            {
                Console.WriteLine("\t\n** {0} {1} successful **", DateStamp.ToString(), (Account.GetType() == typeof(CreditAccount) ? "Payment" : "Deposit"));
                Console.WriteLine(Account.ToString() + "\nAmount deposited: {0:c}", _amount);
            }
            else
            {
                Console.WriteLine("\t\n**{0} ** {1} failed **", DateStamp.ToString(), (Account.GetType() == typeof(CreditAccount) ? "Payment" : "Deposit"));
                Console.WriteLine(Account.ToString());
            }
        }
    }
}
