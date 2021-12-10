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
    /// Specialized class CreditAccount inherits from abstract calss Account
    /// implements abstract deposit and withdraw methods
    /// </summary>
    [DataContract]
    class CreditAccount: Account
    {
        [DataMember]
        public int Limit { get; set; }

        // constructor: initializes base cosntructor and limit
        public CreditAccount(decimal balance, long accNum, int limit): base(balance, accNum)
        {
            Limit = -limit;
        }

        // credit account desposit(payment) and withdraw(purchase) functionality
        /// <summary>
        /// Deposits the payment amount into the account and returns the 
        /// true/false success status for the payment.
        /// </summary>
        /// <param name="amount">the <c>payment</c> to be added to the account</param>
        /// <returns>true/false payment success status</returns>
        public override bool Deposit(decimal? amount = null)
        {
            if (amount.HasValue && amount > 0)
            {
                _balance += (decimal)amount;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Withdraws the purchase amount from the account and return the 
        /// true/false success status for the purchase.
        /// </summary>
        /// <param name="amount">the <c> purchase amount</c> to be withdrawn from the account</param>
        /// <returns>true/false success status of the purchase</returns>
        public override bool Withdraw(decimal amount)
        {
            if (Balance - amount >= Limit)
            {
                _balance -= amount;
                return true;
            }
            return false;
        }

        // returns base ToString() method with Limit details and CreditAccount summary title
        public override string ToString()
        {
            return string.Format("\t\n***** Credit Account Summary *****\n\n" + base.ToString()+"{0}\nLimit: {1:c}", Balance == 0 ? "" : (Balance < 0 ? "Cr" : "Dr"), Math.Abs(Limit));
        }
    }
}
