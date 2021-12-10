using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    /// <summary>
    /// Observer pattern: interface to be implemented by the 
    /// publisher class.
    /// It enables subrscriber to be attached and detached from the
    /// subscribers list and notify them when an event occurs
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Attaches observers to the subscriber list
        /// </summary>
        /// <param name="observer">the subscriber to be added</param>
        void Attach(AccountHolder observer);
        /// <summary>
        /// Detaches observers from the subscribers list
        /// </summary>
        /// <param name="observer">the subscriber to be detached</param>
        void Detach(AccountHolder observer);
        /// <summary>
        /// Notifies all subscribers about the event they have subscribed
        /// for, when it occurs.
        /// </summary>
        void Notify();
    }
}
