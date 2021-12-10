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
    class CreditAccount: Account
    {
        [DataMember]
        public int Limit { get; set; }
        public CreditAccount(decimal balance, string name, int limit): base(balance, name)
        {
            Limit = -limit;
        }
        public override bool Withdraw(decimal amount)
        {
            if (Balance - amount >= Limit)
            {
                _balance -= amount;
                return true;
            }
            return false;
        }
        public override bool Deposit(decimal? amount = null)
        {
            if (amount.HasValue && amount > 0)
            {
                _balance += (decimal)amount;
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return string.Format("\t***** Credit Account Summary *****\n\n" + base.ToString()+"{0}\nLimit: {1:c}", Balance == 0 ? "" : (Balance < 0 ? "Cr" : "Dr"), Math.Abs(Limit));
        }
    }
}
