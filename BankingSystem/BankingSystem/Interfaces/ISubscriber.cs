using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    /// <summary>
    /// Observer pattern: interface to be implemented by the 
    /// subscriber class, that will update subscriber about
    /// an event when it occurs.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Publishes notification about an event, when it occurs, 
        /// which the observer has subscribed to.
        /// </summary>
        /// <param name="subject">The object notifying about the event
        /// and containing information that will be published</param>
        void Update(IPublisher subject);
    }
}
