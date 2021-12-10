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
    class TermDepositAccount: Account
    {
        [DataMember]
        public decimal InterestRate { get; set; }
        [DataMember]
        public int TermPeriod { get; set; }
        public TermDepositAccount(decimal amount, string name, decimal interestRate, int days): base(amount, name)
        {
            InterestRate = interestRate;
            TermPeriod = days;
        }
        public decimal CalculateInterest(int numDays)
        {
            return (Balance * (1 + (InterestRate / 100) * numDays / 365)) - Balance;
        }
        public override bool Deposit(decimal? amount = null)
        {
            if (amount.HasValue && Balance > 0)
            {           
                _balance += (decimal)amount;
                return true;
            }
            else return false;   
        }
        public override bool Withdraw(decimal amount)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return string.Format("\t***** Term deposit account Summary *****\n\n" + base.ToString() + "\nTerm period: {0} days\nInterest rate: {1:0.00}%", TermPeriod, InterestRate);
        }
    }
}
