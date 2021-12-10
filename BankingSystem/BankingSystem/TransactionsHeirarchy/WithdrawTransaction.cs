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
    /// - Withdraw money by calling Withdraw() 
    /// - Rollback withdrawal by calling Deposit()
    /// - Print account summary by calling ToString()
    /// </summary>
    [DataContract]
    class WithdrawTransaction: Transaction
    {
        /// <summary>
        /// This field will be initialized with the account from which the 
        /// withdrawal will be made
        /// </summary>
        [DataMember(Name = "Current_Account_Summary", Order = 2)]
        public Account Account { get; private set; }

        /// <summary>
        /// readonly field stores the post transaction account balance. 
        /// Initialized in the constructor.
        /// </summary>
        [DataMember(Name = "Post_Transaction_Balance", Order = 1)]
        private readonly decimal _postTransactionBalance;

        /// <summary>
        /// Public property to read the success status of the withdrawal
        /// </summary>
        [DataMember]
        public override bool Success
        {
            get { return _success; }
        }       

        // constructor: initializes the account to withdraw from, the amount
        // and set the readonly post transaction balance field.
        public WithdrawTransaction(Account account, decimal amount): base(amount)
        {
            Account = account;
            _postTransactionBalance = Account.Balance - _amount;
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
                bool succeeded = Account.Withdraw(_amount);

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
                    if (_amount <= 0) throw new InvalidOperationException("\nAmount must be greater than zero.");
                    else throw new InvalidOperationException("\nInsufficient funds in the account.");
                }
            }
            // if the withdrawal has already taken place, throws the following exception
            else throw new InvalidOperationException("The transaction has already succesfully executed");
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
                bool succeeded = Account.Deposit(_amount);

                // if rollback succesfull
                if (!succeeded) throw new InvalidOperationException("Rollback failed. Could not deposit funds");
            }
            // if the rollback has already raken place, throw the following exception
            else throw new InvalidOperationException("The rollback transaction has succesfully executed.");
        }

        /// <summary>
        /// Prints the success status of the withdrawal attempt, date stamp and account summary.
        /// </summary>
        public override void Print()
        {
            if (Success)
            {
                Console.WriteLine("\t\n** {0} ** Withdrawal successful **", DateStamp.ToString());
                Console.WriteLine(Account.ToString() + "\nAmount withdrawn: {0:c}", _amount);
            }
            else
            {
                Console.WriteLine("\t\n** {0} ** Withdrawal failed **", DateStamp.ToString());
                Console.WriteLine(Account.ToString());
            }

        }
    }
}
