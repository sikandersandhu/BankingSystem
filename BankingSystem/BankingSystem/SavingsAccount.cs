using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace BankingSystem_Iteration5_AddAccountTypes
{
    [DataContract]
    class SavingsAccount : Account
    {
        public SavingsAccount(decimal amount, string name): base(amount, name) { }
        /// <summary>
        /// Deposits the amount into the account and returns the 
        /// true/false success status for the deposit.
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be added to the account</param>
        /// <returns>true/false deposit success status</returns>
        /// <exception cref="InvalidOperationException">if the amount to be deposited
        /// is not greater than zero</exception>
        public override bool Deposit(decimal? amount = null)
        {
            // if the amount is greater than zero
            if (amount.HasValue && amount > 0)
            {
                // Deposit the funds
                _balance += (decimal)amount;
                // return success status
                return true;
            }
            // else return fail status
            else return false;
        }
        /// <summary>
        /// Withdraws the amount from the account and return the 
        /// true/false success status for the withdrawal.
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be withdrawn from the account</param>
        /// <returns>true/false success status for the withdrawal</returns>
        /// <exception cref="InvalidOperationException">if the <c>amount</c> to be 
        /// withdrawn is less than zero or greater than the current account <c>_balance</c>
        /// </exception>
        public override bool Withdraw(decimal amount)
        {
            // if the amount is greater than zero and not greater than the balance
            if (amount > 0 && amount <= _balance)
            {
                // withdraw the amount
                _balance -= amount;
                // return the success status
                return true;
            }
            // else return fail status
            else return false;
        }
        public override string ToString()
        {
            return string.Format("\t***** Savings account summary *****\n\n" + base.ToString());
        }
    }
}
