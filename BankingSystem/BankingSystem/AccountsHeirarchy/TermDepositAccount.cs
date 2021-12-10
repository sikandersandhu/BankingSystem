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
    /// Specialized class TermDepositAccount, inherits from abstract calss Account
    /// Implements abstract deposit method. Withdraw method overriden, but not implemented.
    /// Provides method to calculate interest and ToString() method to return account summary
    /// </summary>
    [DataContract]
    class TermDepositAccount: Account
    {
        [DataMember(Name ="Interest_Rate")]
        public decimal InterestRate { get; set; }
        [DataMember(Name ="Term_Period")]
        public int TermPeriod { get; set; }

        // constructor: initializes base constructor, interest rate and period of term deposit in days
        public TermDepositAccount(decimal balance, long accNum, decimal interestRate, int days): base(balance, accNum)
        {
            InterestRate = interestRate;
            TermPeriod = days;
        }

        // Methods to calculate interest and deposit interest calculated into the acccount
        /// <summary>
        /// method implements formula to claculate and return interest rate based on the number of days
        /// </summary>
        /// <param name="numDays">the number of days to calculate the interest for</param>
        /// <returns>returns the interest earned on the provided number of days</returns>
        public decimal CalculateInterest(int numDays)
        {
            return (Balance * (1 + (InterestRate / 100) * numDays / 365)) - Balance;
        }
        /// <summary>
        /// if an amount of interest is provided, method deposits the amount 
        /// provided int othe account.
        /// </summary>
        /// <param name="amount">the amount of interest to be added to the account</param>
        /// <returns>true, if succeeded, false, if not succeeded</returns>
        public override bool Deposit(decimal? amount = null)
        {
            if (amount.HasValue && Balance > 0)
            {           
                _balance += (decimal)amount;
                return true;
            }
            else return false;   
        }
        // not implemented: throws NotImplementedException if called
        public override bool Withdraw(decimal amount)
        {
            throw new NotImplementedException();
        }

        // returns base ToString() method with interest rate, term period details and "Term Deposit Account summary" title
        public override string ToString()
        {
            return string.Format("\t\n***** Term deposit account summary *****\n\n" + base.ToString() + "\nTerm period: {0} days\nInterest rate: {1:0.00}%", TermPeriod, InterestRate);
        }
    }
}
