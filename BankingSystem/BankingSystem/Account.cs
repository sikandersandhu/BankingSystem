/**********************************************************************************
 * 
 * File: Account.cs
 * Author/s: Sikander Sandhu
 * Description:
 *          This is the core Account class that forms part of the banking system.
 *          This class allows withdrawl and deposit of money into the account,
 *          and prints the account balance to the terminal.
 *          
 **********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_1
{
    /// <summary>
    /// This is the core <c>Account</c> class that forms part of the banking system.
    /// <para>This class provides a template to create a new account instance. Methods in the class allow
    /// deposit and withdrawal of funds from  the class and prinitng of account summary.</para>
    /// </summary>
    class Account
    {
        /// <summary>
        /// Private instance variable <c>_balance</c> sets the account balance to 0 when a new account object is created.
        /// </summary>
        private decimal _balance;

        /// <summary>
        /// Private instance variable <c>_name</c> sets the name of the account when a new account object is created.
        /// </summary>
        private string _name;

        /// <summary>
        /// <values>Property <c>Name</c> represents the account name</values>
        /// and provides public access to read the account name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// This constructor initializes the new account to
        /// (<paramref name="balance"/>,<paramref name="name"/>).
        /// </summary>
        /// <param name="balance">the new accounts balance.</param>
        /// <param name="name">the new accounts name.</param>
        public Account(decimal balance, string name)
        {
            _balance = balance;
            _name = name;
        }

        /// <summary>
        /// This instance method increments the account <c>_balance</c> with the 
        /// <c>amount</c> requested,
        /// <para> only if the <c>amount</c> is greater than zero.</para>
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be added to the account</param>
        /// <exception cref="InvalidOperationException">if the <c>amount</c> to deposit is less than zero</exception>
        public bool Deposit(decimal amount)
        {
            if (amount > 0)
            {
                _balance += amount;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// This instance method reduces the account <c>_balance</c> with the 
        /// <c>amount</c> requested,
        /// <para> only if the <c>amount</c> is greater than zero and less than the current account <c>_balance.</c></para>
        /// </summary>
        /// <param name="amount">the <c>amount</c> to be withdrawn from the account</param>
        /// <exception cref="InvalidOperationException">if the <c>amount</c> to be 
        /// withdrawn is less than zero or greater than the current account <c>_balance</c>
        /// </exception>
        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount < _balance)
            {
                _balance -= amount;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// This method provides the account summary as a string.
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
            return string.Format("\tAccount Summary\n\n\tAccount name: {0} \n\tAccount balance: {1:c}", _name, _balance);
        }
    }
}
