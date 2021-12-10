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
    /// Account holder to be added to the account
    /// </summary>
    [DataContract(Name ="Account_holder")]
    public class AccountHolder: ISubscriber
    {
        [DataMember(Name ="Account_number")]
        public readonly long AccNum;
        [DataMember(Name = "First name")]
        public string FName { get; private set; }
        [DataMember(Name = "Last_name")]
        public string LName { get; private set; }

        // constructor: initializes the account holder with first, second name
        // and account number
        public AccountHolder(string fName, string lName, long accNum)
        {
            FName = fName;
            LName = lName;
            AccNum = accNum;
        }

        // returns string representation of the account holder
        public override string ToString()
        {
            return string.Format("{0} {1}", FName, LName);
        }

        // updates account holder about an event they have subscribed for
        public void Update(IPublisher subject)
        {
            Console.WriteLine("\n***** Mobile device notification: *****\n");
            (subject as Bank).Transaction.Print();
        }
    }
}
