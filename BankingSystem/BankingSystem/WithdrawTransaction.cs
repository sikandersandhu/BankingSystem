using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_Iteration2
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
    class WithdrawTransaction
    {
        /// <summary>
        /// This field will be initialized with the account from which the 
        /// withdrawal is to be made
        /// </summary>
        private Account _account;
        /// <summary>
        /// This field is initialized with the decimal amount to be withdrawn
        /// from the account
        /// </summary>
        private decimal _amount;
        /// <summary>
        /// Strores the true/false status of the execution of withdrawal
        /// </summary>
        private bool _executed;
        /// <summary>
        /// Public property to read the execution status of withdrawal
        /// </summary>
        public bool Executed
        {
            get { return _executed; }
        }
        /// <summary>
        /// Stores the true/false status of the success of withdrawal
        /// </summary>
        private bool _success;
        /// <summary>
        /// Public property to read the success status of the withdrawal
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }
        /// <summary>
        /// Strores the true/false status of the reversal of the withdrawal transaction
        /// </summary>
        private bool _reversed;
        /// <summary>
        /// public property to read the reversed status of the withdrawal 
        /// </summary>
        public bool Reversed
        {
            get { return _reversed; }
        }

        /// <summary>
        /// This constructor initializes the the <c>_account</c> and <c>_amount</c> fields 
        /// of a new <c>WithdrawTransaction</c> class instance. 
        /// </summary>
        /// <param name="account">The account object to Withdraw amount from</param>
        /// <param name="amount">The amount to be withdrawn from the account</param>
        public WithdrawTransaction(Account account, decimal amount)
        {
            _account = account;
            _amount = amount;
        }

        /// <summary>
        /// Calls the Withdraw method to make a withdrawal from the account.
        /// Prints the status messages and summaries and throws relevant 
        /// error messages if the withdrawal fails.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exceptions is thrown
        /// if the withdrawal failed, or if there are insufficient funds in the 
        /// account, or if the withdrawal has already been executed.</exception>
        public void Execute()
        {
            // If the withdrawal has not yet taken place
            if (!Executed)
            {
                // makes withdrawal and store the status
                bool succeeded = _account.Withdraw(_amount);

                // if withdrawal succesfull
                if (succeeded)
                {
                    // changes the executed and success status to true
                    // prints account summary
                    _executed = true;
                    _success = true;
                    Print();
                }

                // if withdrawal failed
                else
                {
                    // prints the account summary and throw the relevant exception
                    Print();
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
        public void Rollback()
        {
            // If the reversal has not yet taken place
            if (!Reversed)
            {
                // rollsback the withdrawal and saves status
                bool succeeded = _account.Deposit(_amount);

                // if rollback succesfull
                if (succeeded)
                {
                    // changes the status of reversed to true
                    _reversed = true;
                }

                //if rollback failed, throw the following exception
                else throw new InvalidOperationException("Rollback failed. Could not deposit funds");
            }
            // if the rollback has already raken place, throw the following exception
            else throw new InvalidOperationException("The amount has already been succesfully deposited.");
        }

        /// <summary>
        /// Prints the success or failed status of the withdrawal attempt and prints the account summary.
        /// </summary>
        public void Print()
        {
            if (Success)
            {
                Console.WriteLine("\n***** Withdrawal successful *****\n");
                Console.WriteLine(_account.ToString());
            }
            else
            {
                Console.WriteLine("\n***** Withdrawal failed *****\n");
                Console.WriteLine(_account.ToString());
            }
        }
    }
}
