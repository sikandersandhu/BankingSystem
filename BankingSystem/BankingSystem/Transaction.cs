using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_Iteration3
{
    abstract class Transaction
    {
        /// <summary>
        /// This protected field is initialized with the decimal amount 
        /// for the transaction (only accesible by the derived class)
        /// </summary>
        protected decimal _amount;

        /// <summary>
        /// Protected field stores the true/false status of the success of transaction
        /// ( only accessible to the derived classes )
        /// </summary>
        /// 
        protected bool _success;   
        /// <summary>
        /// To read the true/false success status of the transaction
        /// Abstract property to be set in the derived class. 
        /// </summary>
        public abstract bool Success { get; }

        /// <summary>
        /// Strores the true/false status of the execution of the transaction
        /// </summary>
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
        private bool _reversed;
        /// <summary>
        /// public property to read the reversed status of the transaction
        /// </summary>
        public bool Reversed
        {
            get { return _reversed; }
        }

        /// <summary>
        /// Stores the date and time when the transaction took place
        /// </summary>
        private DateTime _dateStamp;
        /// <summary>
        /// Public property to read the date and time of the transaction
        /// </summary>
        public DateTime DateStamp
        {
            get { return _dateStamp; }
        }

        public Transaction ( decimal amount)
        {
            _amount = amount;
        }

        public abstract void Print();

        public virtual void Execute()
        {
            _dateStamp = DateTime.Now;
            _executed = true;
        }

        public virtual void Rollback()
        {
            _dateStamp = DateTime.Now;
            _reversed = true;
        }
    }
}
