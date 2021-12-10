using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace BankingSystem_Iteration5_AddAccountTypes
{
    /// <summary>
    /// This is the core <c>Account</c> class that forms part of the banking system.
    /// It includes methods that deposit and withdraw funds and print account summary.
    /// </summary>
    [DataContract]
    [KnownType(typeof(SavingsAccount))]
    [KnownType(typeof(TermDepositAccount))]
    [KnownType(typeof(CreditAccount))]
    abstract class Account
    {
        /// <summary>
        /// The field to be initialized with the initial account balance
        /// </summary>
        [DataMember(Name = "Balance")]
        protected decimal _balance;
        public decimal Balance
        {
            get => _balance;
        }
        /// <summary>
        /// The field to be initialized with the account name
        /// </summary>
        [DataMember(Name = "Account_Name")]
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
        protected Account(decimal balance, string name)
        {
            _balance = balance;
            _name = name;
        }
        public Account(Account account)
        {
            _balance = account.Balance;
            _name = account.Name;
        }
        public abstract bool Deposit(decimal? amount = null);
        public abstract bool Withdraw(decimal amount);      
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
            return string.Format("Account name: {0}\nAccount balance: {1:c}",_name, Math.Abs(_balance));
        }
    }
}
