using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    /// <summary>
    /// Generic interface for adding, removing and finding objects in a list
    /// </summary>
    interface IListAccess
    {
        /// <summary>
        /// Generic method: adds a user defined type of object to the list
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="toAdd">the object reference to add to the list</param>
        void Add<T>(T toAdd);

        /// <summary>
        /// Generic method: removes a user defined type of object from the list
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="toAdd">the object reference to remove from the list</param>
        void Rem<T>(T toRem);

        /// <summary>
        /// Generic method: finds and returns a user defined type of object reference or null
        /// </summary>
        /// <typeparam name="T">The list Type</typeparam>
        /// <param name="list">array of parameters needed to evaluate and return the matching object</param>
        /// <returns>reference of the matching object or null if not matches found</returns>
        T Find<T>(params object[] list) where T : class;
    }
}
