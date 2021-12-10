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
    /// This is the core abstract <c>Account</c> class that forms part of the banking system.
    /// It includes abstract methods that deposit and withdraw funds, methods that add, remove and find
    /// account holders in the account and return account summary.
    /// Specialized by SavingsAccount, CreditAccount, TermDepositAccount
    /// </summary>
    [DataContract]
    [KnownType(typeof(SavingsAccount))]
    [KnownType(typeof(TermDepositAccount))]
    [KnownType(typeof(CreditAccount))]
    [KnownType(typeof(List<AccountHolder>))]
    abstract class Account: IListAccess
    {
        [DataMember(Name ="Account_number")]
        public readonly long AccNum;
        /// <summary>
        /// The field to be initialized with the initial account balance
        /// </summary>
        [DataMember(Name = "Balance")]
        protected decimal _balance;
        public decimal Balance
        {
            get => _balance;
        }
        [DataMember(Name = "Account_Holders")]
        public List<AccountHolder> AccHolders { get; private set; }

        // protected constructor, 
        /// <summary>
        /// This constructor initializes the account name and balance fields.
        /// </summary>
        /// <param name="balance">the initial accounts balance.</param>
        /// <param name="name">name of the account</param>
        protected Account(decimal balance, long accNum)
        {
            _balance = balance;
            AccNum = accNum;
            AccHolders = new List<AccountHolder>();
        }

        // abstract methods for deposit and withdrawal from account
        public abstract bool Deposit(decimal? amount = null);
        public abstract bool Withdraw(decimal amount);

        // methods to add, remove and find in the AccountHolders list
        public void Add<T>(T toAdd)
        {
            if (toAdd.GetType() != typeof(AccountHolder)) throw new ArgumentException("\n***** InvalidArgument: Reference of type AccountHolder expected *****\n");
            if (AccHolders == null) throw new NullReferenceException("\n***** NullReference: Account holder list empty *****\n");
            if (AccHolders.Contains(toAdd as AccountHolder)) throw new InvalidOperationException("\n***** Failed to add account: AN account holder with the provided details already exists *****\n");

            AccHolders.Add(toAdd as AccountHolder);
            Console.WriteLine("\n***** Success: {0} added *****\n", toAdd.GetType().Name.ToString());
        }
        public void Rem<T>(T toRem)
        {
            if (toRem.GetType() != typeof(AccountHolder)) throw new ArgumentException("\n***** Invalid argument: Reference of type AccountHolder expected *****\n");
            if (AccHolders == null) throw new NullReferenceException("\n***** NullReference: Account holder list empty *****\n");
            if (!AccHolders.Contains(toRem as AccountHolder)) throw new InvalidOperationException("\n***** Failed to remove account: An account holder with the provided details does not exist *****\n");

            AccHolders.Remove(toRem as AccountHolder);
            Console.WriteLine("\n***** Success: {0} removed *****\n", toRem.GetType().Name);
        }
        public T Find<T>(params object[] list) where T: class
        {
            var accNum = list[0];
            var fName = list[1];
            var lName = list[2];

            if (fName.GetType() != typeof(string) | lName.GetType() != typeof(string)) throw new ArgumentException("\n***** Invalid argument: first name and last name as string expected *****\n");
            if (fName == null ||  lName == null) throw new NullReferenceException("\n***** Infomation missing: Both first and last names are required. *****\n");
            if (AccHolders == null) throw new NullReferenceException("\n***** NullReference: Account holder list empty *****\n");

            AccountHolder toFind = null;
        
            foreach (AccountHolder accHolder in AccHolders) if (accHolder.AccNum == (long)accNum  &&  accHolder.FName == (string)fName && accHolder.LName == (string)lName) toFind = accHolder;

            return toFind as T;
        }
        
        // Methods to print account summary
        /// <summary>
        /// Provides the account summary as a string.
        /// </summary>
        /// <returns>a formated string representation of the account name\s and balance.
        /// <example>For example:
        /// <c>       
        /// Account names: John Do | John Lo |
        /// Account balance: $500 
        /// </c>
        /// </example></returns>
        /// 
        public override string ToString()
        {           
            return string.Format(PrintAccHolders() + "\nAccount number: {0}\nAccount balance: {1:c}", AccNum, Math.Abs(_balance));
        }
        private string PrintAccHolders()
        {
            string accHolders = string.Format("Account holder{0}: ", AccHolders.Count == 1 ? "" : "s");
            foreach (AccountHolder accHolder in AccHolders) accHolders += string.Format(accHolder.ToString() + " | ");
            return accHolders;
        }
    }
}
