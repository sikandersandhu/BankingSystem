using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace BankingSystem_Iteration4_serializing
{
    /// <summary>
    /// The methods in this class merely call methods from the account class to 
    /// carry out operations on the account.
    /// The methods also provide status messages and throw relevant exceptions to the user.
    /// These methods perform the following operations:
    /// - Withdraw money by calling Withdraw() 
    /// - Rollback withdrawal by calling Deposit()
    /// - Print account summary by calling ToString()
    /// </summary>
    [DataContract]
    class WithdrawTransaction: Transaction
    {
        /// <summary>
        /// This field will be initialized with the account from which the 
        /// withdrawal is to be made
        /// </summary>
        [DataMember(Name = "Account")]
        private Account _account;

        /// <summary>
        /// Stores the account balance snapshot 
        /// during the object initialization.
        /// Initialized in the constructor.
        /// </summary>
        [DataMember(Name = "Post_Transaction_Balance")]
        private readonly decimal _postTransactionBalance;

        /// <summary>
        /// Public property to read the success status of the withdrawal
        /// </summary>
        [DataMember]
        public override bool Success
        {
            get { return _success; }
        }
        
        /// <summary>
        /// This constructor initializes the the <c>_account</c> and <c>_amount</c> fields 
        /// of a new <c>WithdrawTransaction</c> class instance. 
        /// </summary>
        /// <param name="account">The account object to Withdraw amount from</param>
        /// <param name="amount">The amount to be withdrawn from the account</param>
        public WithdrawTransaction(Account account, decimal amount): base(amount)
        {
            _account = account;
            _postTransactionBalance = _account.Balance - _amount;
        }

        /// <summary>
        /// Calls the Withdraw method to make a withdrawal from the account.
        /// Prints the status messages and summaries and throws relevant 
        /// error messages if the withdrawal fails.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exceptions is thrown
        /// if the withdrawal failed, or if there are insufficient funds in the 
        /// account, or if the withdrawal has already been executed.</exception>
        public override void Execute()
        {
            // If the withdrawal has not yet taken place
            if (!Executed)
            {
                base.Execute();
                // makes withdrawal and store the status
                bool succeeded = _account.Withdraw(_amount);

                // if withdrawal succesfull
                if (succeeded)
                {
                    // changes the executed and success status to true
                    // prints account summary
                    _success = true;
                }

                // if withdrawal failed
                else
                {
                    // prints the account summary and throw the relevant exception
                    if (_amount <= 0) throw new InvalidOperationException("\nWithdrawal amount must be greater than zero.");
                    else throw new InvalidOperationException("\nInsufficient funds in the account. Try a different amount.");
                }
            }
            // if the withdrawal has already taken place, throws the following exception
            else throw new InvalidOperationException("Withdrawal has already been succesfully executed");
        }

        /// <summary>
        /// Calls the Deposit method to Rolls back a withdrawal from
        /// the account.Prints the status, and account summary 
        /// and throws relevant exceptions if the rollback fails.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exception is thrown if the 
        /// reversal fails, or if the reversal has already been executed</exception>
        public override void Rollback()
        {
            // If the reversal has not yet taken place
            if (!Reversed)
            {
                // changes the status of reversed to true
                base.Rollback();
                // rollsback the withdrawal and saves status
                bool succeeded = _account.Deposit(_amount);

                // if rollback succesfull
                if (!succeeded) throw new InvalidOperationException("Rollback failed. Could not deposit funds");
            }
            // if the rollback has already raken place, throw the following exception
            else throw new InvalidOperationException("The amount has already been succesfully deposited.");
        }

        /// <summary>
        /// Prints the success or failed status of the withdrawal attempt and prints the account summary.
        /// </summary>
        public override void Print()
        {
            if (Success)
            {
                Console.WriteLine("\t***** Account Summary *****\n");
                Console.WriteLine("** {0} ** Withdrawal successful **\n", DateStamp.ToString());
                Console.WriteLine("Account name: {0}\nWithdrawal amount: {1:c}\nAccount balance: {2:c}", _account.Name, _amount, _postTransactionBalance);
            }
            else
            {
                Console.WriteLine("\t***** Account Summary *****\n");
                Console.WriteLine("{0}  ** Withdrawal failed **\n", DateStamp.ToString());
                Console.WriteLine(_account.ToString());
            }
        }
    }
}
