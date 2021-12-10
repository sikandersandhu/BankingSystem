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
    /// The methods in this class merely (try) call methods from the WithdrawTransaction class 
    /// and the DepositTransaction class to make a transfer between two accounts and (catch)
    /// any exceptions and display the error to the user, in case of failure in the transaction
    /// </summary>
    [DataContract]
    class TransferTransaction: Transaction
    {
        /// <summary>
        /// Fields to be initialized with the fromAccount and toAccount objects
        /// to make the transfer.
        /// </summary>
        [DataMember(Name = "From_Account")]
        private Account _fromAccount;

        [DataMember(Name = "To_Account")]
        private Account _toAccount;

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
        [DataMember]
        public override bool Success
        {
            get { return _success; }
        }

        /// <summary>
        /// Stores the from and to account balance 
        /// snapshot during the object initialization.
        /// </summary>
        [DataMember(Name = "Post_Transaction_Balance_From_Account")]
        private readonly decimal _PostTransactionBalanceFromAccount;

        [DataMember(Name = "Post_Transaction_Balance_To_Account")]
        private readonly decimal _PostTransactionBalanceToAccount;

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
        public TransferTransaction(Account fromAccount, Account toAccount, decimal amount): base(amount)
        {
            _fromAccount = fromAccount;
            _toAccount = toAccount;
            _PostTransactionBalanceFromAccount = _fromAccount.Balance - _amount;
            _PostTransactionBalanceToAccount = _toAccount.Balance + _amount;
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
        public override void Execute()
        {
            // if the transfer has not already been executed
            if (!Executed)
            {
                base.Execute();
                // tries to make withdrawal
                try
                {
                    _withdraw.Execute();
                    _deposit.Execute();
                    _success = true;
                }

                // if transfer fails, relevant exceptions are cought
                // and displayed to the user
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }
            }
            // if the transfer was already executed, throw the following exception.
            else throw new InvalidOperationException("A transfer has previously been attempted.");
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
        public override void Rollback()
        {
            // if the transfer has succesfully executed and rollback 
            // has not yet occured
            if (Executed && !Reversed)
            {
                base.Rollback();
                // tries to rollback deposit
                try
                {
                    _deposit.Rollback();
                    _withdraw.Rollback();
                }
                // if rollback fails, throw relevant exceptiosn
                catch (InvalidOperationException ioe)
                {
                    if (_deposit.Reversed) _deposit.Execute();
                    Console.WriteLine(ioe.Message);
                }
            } 
            // if transfer did not take place sucessfully, throw the relevant exception.
            // if the transfer has already succesfully been rolled back, throw relevant exception.
            else 
            {
                if (!Executed) throw new InvalidOperationException("A transfer was never attempted.");
                if (Reversed) throw new InvalidOperationException("A rollback has previously been attempted");
            }                
        }

        /// <summary>
        /// Prints the from account and to account summaries
        /// </summary>
        public override void Print()
        {
            if (Success)
            {
                Console.WriteLine("\t***** Account Summary *****\n");
                Console.WriteLine("** {0} ** Transfer successful **\n", DateStamp.ToString());
                Console.WriteLine("\n{0:c} transferred: \n\nFrom account: {1}\nBalance: {2:c}\n\nTo account: {3}\nBalance: {4:c}\n", _amount, _fromAccount.Name, _PostTransactionBalanceFromAccount, _toAccount.Name, _PostTransactionBalanceToAccount);
            }
            else
            {
                Console.WriteLine("\t***** Account Summary *****\n");
                Console.WriteLine("{0}  ** Transfer failed **\n", DateStamp.ToString());
                Console.WriteLine("\nFrom account: {0}\nBalance: {1:c}\n\nTo account: {2}\nBalance: {3:c}\n", _fromAccount.Name, _PostTransactionBalanceFromAccount + _amount, _toAccount.Name, _PostTransactionBalanceToAccount - _amount);
            }
        }
    }
}
