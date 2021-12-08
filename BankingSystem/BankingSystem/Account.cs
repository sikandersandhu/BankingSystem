using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_Iteration3
{
    /// <summary>
    /// This is the core <c>Account</c> class that forms part of the banking system.
    /// It includes methods that deposit and withdraw funds and print account summary.
    /// </summary>
    class Account
    {
        /// <summary>
        /// The field to be initialized with the initial account balance
        /// </summary>
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }
        }

        /// <summary>
        /// The field to be initialized with the account name
        /// </summary>
        private string _name;

        /// <summary>
        /// Public property to read account name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        /// <summary>
        /// This constructor initializes the account name and balance fields.
        /// </summary>
        /// <param name="balance">the initial accounts balance.</param>
        /// <param name="name">name of the account</param>
        public Account(decimal balance, string name)
        {
            _balance = balance;
            _name = name;
        }

        /// <summary>
        /// Deposits the amount into the account and returns the 
        /// true/false success status for the deposit.
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be added to the account</param>
        /// <returns>true/false deposit success status</returns>
        /// <exception cref="InvalidOperationException">if the amount to be deposited
        /// is not greater than zero</exception>
        public bool Deposit(decimal amount)
        {
            // if the amount is greater than zero
            if (amount > 0)
            {
                // Deposit the funds
                _balance += amount;
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
        public bool Withdraw(decimal amount)
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

        /// <summary>
        /// Provides the account summary as a string.
        /// </summary>
        /// <returns>a formated string representation of the account balance.
        /// <example>For example:
        /// <c>
        ///        Account Summary
        ///        
        /// Account name: John Do
        /// Account balance: $500 
        /// </c>
        /// </example></returns>
        public override string ToString()
        {
            return string.Format("\t***** Account Summary *****\n\nAccount name: {0}\nAccount balance: {1:c}",_name, _balance);
        }
    }
}
