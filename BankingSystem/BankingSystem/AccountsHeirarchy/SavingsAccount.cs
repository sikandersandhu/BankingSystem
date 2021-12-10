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
    /// Specialized class SavingsAccount inherits from abstract class Account.
    /// Implements abstract deposit and withdraw methods.
    /// </summary>
    [DataContract]
    class SavingsAccount : Account
    {
        // Constructor; initializes base constructor in Account class.
        public SavingsAccount(decimal balance, long accNum): base(balance, accNum) { }

        // savings account desposit and withdraw functionality
        /// <summary>
        /// Deposits the amount into the account and returns the 
        /// true/false success status for the deposit.
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be added to the account</param>
        /// <returns>true/false deposit success status</returns>
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

        // returns base ToString() method with Savingsaccount summary title
        public override string ToString()
        {
            return string.Format("\t\n***** Savings account summary *****\n\n" + base.ToString());
        }
    }
}
