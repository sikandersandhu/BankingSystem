using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace BankingSystem_Iteration5_AddAccountTypes
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
        private Account _account;
        /// <summary>
        /// Stores the account balance snapshot 
        /// during the object initialization.
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
        /// <summary>
        /// This constructor initializes the the <c>_account</c> and <c>_amount</c> fields 
        /// of a new <c>DespsitTransaction</c> class instance. 
        /// </summary>
        /// <param name="account">The account to deposit amount into</param>
        /// <param name="amount">The amount to be deposited in the account</param>
        public DepositTransaction(Account account, decimal amount) : base(amount)
        {
            _account = account;
            _postTransactionBalance = _account.Balance + _amount;
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
                bool succeeded = _account.Deposit(_amount);

                // If deposit succeeded
                if (succeeded)
                {
                    // changes executed and success status to true
                    // and prints account summary                   
                    _success = true;
                }
                // If deposit failed
                else
                {
                    throw new InvalidOperationException("Amount must be greater than zero.");
                }
            }
            // if the deposit has already been executed,
            // throws the following error message
            else
            {
                throw new InvalidOperationException("The transaction has already been succesfully executed.");
            }
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
                bool succeeded = _account.Withdraw(_amount);

                // If deposit succeeded
                if (!succeeded) throw new InvalidOperationException("Rollback failed. Not enough funds in the acccount.");
            }
            // If the deposit has already been reversed, thorws the following exception
            else throw new InvalidOperationException("The amount has already been succesfully rolledback.");
        }
        /// <summary>
        /// Prints the success or failed status of the deposit attempt and prints the account summary
        /// </summary>
        public override void Print()
        {
            if (Success)
            {
                Console.WriteLine("\t** {0} ** Deposit successful **\n", DateStamp.ToString());
                Console.WriteLine(_account.ToString() + "\nAmount deposited: {0:c}", _amount);
            }
            else
            {
                Console.WriteLine("\t**{0} ** Deposit failed **\n", DateStamp.ToString());
                Console.WriteLine(_account.ToString());
            }
        }
    }
}
