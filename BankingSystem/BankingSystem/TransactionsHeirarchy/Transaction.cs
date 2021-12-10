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
    /// Base abstract transaction class specialized by the DepositTransaction. WithdrawTransaction
    /// and TransferTransaction classes.
    /// Provided functionality to Executes, rollback and print transaction record
    /// </summary>
    [DataContract]
    [KnownType(typeof(WithdrawTransaction))]
    [KnownType(typeof(DepositTransaction))]
    [KnownType(typeof(TransferTransaction))]
    public abstract class Transaction
    {
        /// <summary>
        /// This protected field is initialized with the decimal amount 
        /// for the transaction (only accesible by the derived class)
        /// </summary>
        [DataMember(Name = "Transaction_Amount")]
        protected decimal _amount;
        /// <summary>
        /// Protected field stores the true/false status of the success of transaction
        /// ( only accessible to the derived classes )
        /// </summary>
        ///         
        [DataMember(Name = "Success")]
        protected bool _success;   
        /// <summary>
        /// To read the true/false success status of the transaction
        /// Abstract property to be set in the derived class. 
        /// </summary>
        public abstract bool Success { get; }
        /// <summary>
        /// Strores the true/false status of the execution of the transaction
        /// </summary>
        [DataMember(Name = "Executed", IsRequired = false)]
        private bool _executed;
        /// <summary>
        /// Public property to read the execution status of the transaction
        /// </summary>
        public bool Executed
        {
            get { return _executed; }
        }
        /// <summary>
        /// Strores the true/false status of the reversal of the transaction
        /// </summary>
        [DataMember(Name = "Reversed", IsRequired = false)]
        private bool _reversed;
        /// <summary>
        /// public property to read the reversed status of the transaction
        /// </summary>
        public bool Reversed
        {
            get { return _reversed; }
        }
        /// <summary>
        /// Stores the date and time when the transaction takes place
        /// </summary>
        [DataMember(Name = "Date_Time")]
        private DateTime _dateStamp;
        /// <summary>
        /// Public property to read the date and time of the transaction
        /// </summary>
        public DateTime DateStamp
        {
            get { return _dateStamp; }
        }

        // protected constructor: initializes transaction amount. 
        // Instanciated through the specialized Deposit, Withdraw and Transfer
        // Transaction classes
        protected Transaction ( decimal amount)
        {
            _amount = amount;
        }

        /// <summary>
        /// Virtual method sets date time and execution status of the transaction.
        /// Rest of the implementation in the overidden Execute() method in the
        /// specialized class
        /// </summary>
        public virtual void Execute()
        {
            _dateStamp = DateTime.Now;
            _executed = true;
        }

        /// <summary>
        /// Virtual method sets date time and reversed status of the transaction.
        /// Rest of the implementation in the overidden Rollback() method in the
        /// specialized class
        /// </summary>
        public virtual void Rollback()
        {
            _dateStamp = DateTime.Now;
            _reversed = true;
        }

        /// <summary>
        /// Abstract method to print the transaction details to be overriden in specialized classes
        /// </summary>
        public abstract void Print();
    }
}
