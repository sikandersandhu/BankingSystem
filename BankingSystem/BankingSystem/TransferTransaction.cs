using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_Iteration2
{
    /// <summary>
    /// The methods in this class merely (try) call methods from the WithdrawTransaction class 
    /// and the DepositTransaction class to make a transfer between two accounts and (catch)
    /// any exceptions and display the error to the user, in case of failure in the transaction
    /// </summary>
    class TransferTransaction
    {
        /// <summary>
        /// Fields to be initialized with the fromAccount and toAccount objects
        /// to make the transfer.
        /// </summary>
        private Account _fromAccount, _toAccount;
        /// <summary>
        /// Field to be initialized with the DepositTransaction object
        /// to make the deposit.
        /// </summary>
        private DepositTransaction _deposit;
        /// <summary>
        /// Field to be initialized with the WithdrawTransaction object
        /// to make the withdrawal
        /// </summary>
        private WithdrawTransaction _withdraw;
        /// <summary>
        /// The field to be initialized with the decimal amount to be transferred
        /// </summary>
        private decimal _amount;
        /// <summary>
        /// Field to store the true/false execution status of the transfer
        /// </summary>
        private bool _executed;
        /// <summary>
        /// public property to read execution status of the transfer
        /// </summary>
        public bool Executed
        {
            get { return _executed; }
        }
        /// <summary>
        /// field to store the true/false reversed status of the transfer.
        /// </summary>
        private bool _reversed;
        /// <summary>
        /// public property to read the reversed status of the transfer
        /// </summary>
        public bool Reversed
        {
            get { return _reversed; }
        }

        /// <summary>
        /// This construtor initializes the <c>_fromAccount</c>, <c>_toAccount</c> 
        /// and <c>_amount</c> fields using the parameters passed to the constructor
        /// and creates an new objects of <c>WithdrawTransaction</c> and 
        /// <c>DepositTransaction</c> and assign these to <c>_withdraw</c> and 
        /// <c>_deposit</c>fields resepectively.
        /// </summary>
        /// <param name="fromAccount">the account to transfer funds from</param>
        /// <param name="toAccount">the account to transfer funds to</param>
        /// <param name="amount">the amount to be transfered </param>
        public TransferTransaction(Account fromAccount, Account toAccount, decimal amount)
        {
            _fromAccount = fromAccount;
            _toAccount = toAccount;
            _amount = amount;
            _withdraw = new WithdrawTransaction(fromAccount, amount);
            _deposit = new DepositTransaction(toAccount, amount);
        }

        /// <summary>
        /// Calls the WithdrawTransaction and DepositTransaction class Execute() 
        /// methods to make the transfer. If the transfer fails, the relevant 
        /// exceptions are caught and presented to the use and the needed
        /// rollbacks are performed
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws this exception if the 
        /// transfer has already succesfully executed and catches and shows this 
        /// except if the transfer fails</exception>
        public void Execute()
        {
            // if the transfer has not already been executed
            if (!Executed)
            {
                // tries to make withdrawal
                try
                {
                    _withdraw.Execute();
                }

                // if transfer fails, relevant exceptions are cought
                // and displayed to the user
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("\n***** Transfer Failed *****\n");
                    Console.WriteLine(ioe.Message);
                }

                // If withdrawal is succesful
                if (_withdraw.Success)
                {
                    // tries to make deposit
                    try
                    {
                        _deposit.Execute();
                    }

                    // if deposit fails, rollsback withdrawal, catches exception
                    // and displays it to the user
                    catch (InvalidOperationException ioe)
                    {
                        Console.WriteLine("\n***** Transfer Failed *****\n");
                        _withdraw.Rollback();
                        Console.WriteLine(ioe.Message);
                    }
                }

                // if bothe withdrawal and deposit are succesfull,
                // changes the status of executed to true.
                if (_withdraw.Success && _deposit.Success)  _executed = true; 

                }

                // if the transfer was already executed, throw the following exception.
                else throw new InvalidOperationException("The transfer has already succesfully executed.");
        }

        /// <summary>
        /// Calls the WithdrawTransaction and DepositTransaction class Rollback() 
        /// methods to rollback the transfer. If the rollback fails, the relevant 
        /// exceptions are caught and presented to the uses, the needed
        /// rollbacks are performed and new relevant exceptions thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws this exception if the 
        /// transfer has already succesfully executed or if the rollback has already
        /// succesfully occured.
        /// Catches the relevant exceptions, if the rollback fails </exception>
        public void Rollback()
        {
            // if the transfer has succesfully executed and rollback 
            // has not yet occured
            if (Executed && !Reversed)
            {
                // tries to rollback deposit
                try
                {
                    _deposit.Rollback();
                }

                // if rollback fails, throw relevant exceptiosn
                catch(InvalidOperationException ioe)
                {
                    Console.WriteLine("\n***** Rollback Failed *****\n");
                    Console.WriteLine(ioe.Message);
                }
                
                // if deposit succesfully rolled back
                if(_deposit.Reversed)
                {
                    // tries to rollback Withdrawal
                    try
                    {
                        _withdraw.Rollback();
                    }
                    
                    // if the rollback fails, 
                    catch (InvalidOperationException ioe)
                    {
                        // deposit the rolled back deposit again,
                        // and catch and show the relevant exception.
                        _deposit.Execute();
                        Console.WriteLine("\n***** Rollback Failed *****\n");
                        Console.WriteLine(ioe.Message);
                    }
                }   
                
                // if both deposit and withdrawal succesfully rolled back,
                // change reversed status to true
                if (_deposit.Reversed && _withdraw.Reversed) _reversed = true;
            }

            // if transfer did not take place sucessfully, throw the relevant exception.
            // if the transfer has already succesfully been rolled back, throw relevant exception.
            else 
            {
                if (!Executed) throw new InvalidOperationException("The transfer was not succesfull.");
                if (Reversed) throw new InvalidOperationException("The reversal has already executed succesfully");
            }                
        }

        /// <summary>
        /// Prints the from account and to account summaries
        /// </summary>
        public void Print()
        {
            // Summaries is the transfer succeeds
            if (_withdraw.Success && _deposit.Success)
            {
                Console.WriteLine("***** Transfer successful *****\n");
                Console.WriteLine(_fromAccount.ToString());
                Console.WriteLine("\n");
                Console.WriteLine(_toAccount.ToString());
            }

            // Summaries if the transfer failed
            else
            {
                Console.WriteLine("***** Transfer failed *****\n");
                Console.WriteLine(_fromAccount.ToString());
                Console.WriteLine("\n");
                Console.WriteLine(_toAccount.ToString());
            }
        }
    }
}
