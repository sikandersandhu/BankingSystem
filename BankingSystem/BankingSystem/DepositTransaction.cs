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
    /// - Deposit money by calling Deposit() 
    /// - Rollback deposit by calling Withdraw()
    /// - Print account summary by calling ToString()
    /// </summary>
    class DepositTransaction
    {
        /// <summary>
        /// This field will be initialized with the account in which the 
        /// deposit is to be made
        /// </summary>
        private Account _account;
        /// <summary>
        /// This field is initialized with the decimal amount to be deposited
        /// to the account
        /// </summary>
        private decimal _amount;
        /// <summary>
        /// Strores the true/false status of the execution of deposit
        /// </summary>
        private bool _executed;
        /// <summary>
        /// Public property to read the execution status of deposit
        /// </summary>
        public bool Executed
        {
            get { return _executed; }
        }
        /// <summary>
        /// Stores the true/false status of the success of deposit
        /// </summary>
        private bool _success;
        /// <summary>
        /// Public property to read the success status of the deposit
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }
        /// <summary>
        /// Strores the true/false status of the reversal of deposit transaction
        /// </summary>
        private bool _reversed;
        /// <summary>
        /// public property to read the reversed status of the deposit 
        /// </summary>
        public bool Reversed
        {
            get { return _reversed; }
        }

        /// <summary>
        /// This constructor initializes the the <c>_account</c> and <c>_amount</c> fields 
        /// of a new <c>DespsitTransaction</c> class instance. 
        /// </summary>
        /// <param name="account">The account to deposit amount into</param>
        /// <param name="amount">The amount to be deposited in the account</param>
        public DepositTransaction(Account account, decimal amount)
        {
            _account = account;
            _amount = amount; 
        }

        /// <summary>
        /// Calls the deposit method to make a deposit into the account.
        /// Prints the status messages and summaries and throws relevant 
        /// error messages if the deposit fails.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exceptions is thrown if the deposit failed.</exception>
        public void Execute()
        { 
            // If the deposit has not been executed
            if (!Executed)
            {
                // executes the deposit and store the status               
                bool succeeded = _account.Deposit(_amount);

                // If deposit succeeded
                if (succeeded)
                {
                    // changes executed and success status to true
                    // and prints account summary
                    _executed = true;
                    _success = true;
                    Print();
                }

                // If deposit failed
                else
                {
                    // prints account summary and throws the
                    // following error message.
                    Print();
                    throw new InvalidOperationException("Deposit amount must be greater than zero.");
                }
            }  
            // if the deposit has already been executed,
            // throws the following error message
            else throw new InvalidOperationException("The Deposit has already been succesfully executed.");
        }

        /// <summary>
        /// Calls the withdraw method to Rolls back a deposit from
        /// the account.Prints the status, and account summary 
        /// and throws relevant exceptions.
        /// </summary>
        /// <exception cref="InvalidOperationException">This exception is thrown if the reversal fails</exception>
        /// <exception cref="InvalidOperationException">This exception is thrown if the reversal has 
        /// already been succuesfully executed.</exception>
        public void Rollback()
        {
            // If the depsoit has not been reversed
            if (!Reversed)
            {
                //Reverses deposit and stores the status
                bool succeeded = _account.Withdraw(_amount);

                // If deposit succeeded
                if (succeeded)
                {
                    // changes reverse status to true
                    _reversed = true;
                }
                // otherwise, throws the following exception
                else throw new InvalidOperationException("Rollback failed. Not enough funds in the acccount to make a withdrawl.");
            }
            // If the deposit has already been reversed, thorws the following exception
            else throw new InvalidOperationException("The amount has already been succesfully rolledback.");
        }

        /// <summary>
        /// Prints the success or failed status of the deposit attempt and prints the account summary
        /// </summary>
        public void Print()
        {
            if (Success)
            {
                Console.WriteLine("\n***** Deposit successful *****\n");
                Console.WriteLine(_account.ToString());
            }
            else
            {
                Console.WriteLine("\n***** Deposit failed *****\n");
                Console.WriteLine(_account.ToString());
            }
        }
    }
}
