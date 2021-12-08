using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem_Iteration2
{   /// <summary>
/// The instance from this class has two roles. 
/// First, to add new accounts to the 
/// the List of accounts and to find out if an account already exists in the list or not.
/// The methods either returns the found account reference or a null reference if
/// account not found.
/// Second, The overriden ExecuteTransaction method, calls methods from the DepositTransaction,
/// WithdrawTransaction and TransferTransaction classes to execute transaction.
/// </summary>
    class Bank
    {
        /// <summary>
        /// this field is a list of type Account, into which accounts will be added
        /// </summary>
        private List<Account> _accounts;

        /// <summary>
        /// This constructor initializes a new list of Account type
        /// </summary>
        public Bank()
        {
            _accounts = new List<Account>();
        }

        /// <summary>
        /// Adds a new account to the list
        /// </summary>
        /// <param name="account">The account to be added</param>
        /// <exception cref="InvalidOperationException">if the account with the name already exists</exception>
        public void AddAccount(Account account)
        {
            // if the list of account is not empty
            if(_accounts != null)
            {
                // Check each account
                foreach(Account acc in _accounts)
                {
                    // if the account already exist throw an exception
                    if (acc.Name == account.Name) throw new InvalidOperationException("An account with this name already exists.");
                }
            }
            // if the account does not exist, add the account to the list.
            _accounts.Add(account);
        }

        /// <summary>
        /// Using the provided account name, searches the list of accounts
        /// and returns the result
        /// </summary>
        /// <param name="name">name of the account to search for</param>
        /// <returns>account reference if account found or null reference if account not found</returns>
        public Account GetAccount(string name)
        {
            //account with a null reference to assign the search result to
            Account account = null;

            // If the list is not empty
            if (_accounts != null)
            {   
                // look through the list
                foreach(Account acc in _accounts)
                {
                    // If the account exists, assign it to account variable
                    if (acc.Name == name) account = acc;
                }
            }
            // if account found, returns account reference, else account with null reference
            return account;            
        }

        /// <summary>
        /// Calls the Execute method to deposit funds, using the DepositTransaction object.
        /// </summary>
        /// <param name="deposit">Initialized DepositTransaction object, to be used to execute deposit</param>
        public void ExecuteTransaction(DepositTransaction deposit)
        {
            deposit.Execute();
        }

        /// <summary>
        /// Calls the Execute method to withdraw funds, using the WithdrawTransaction object.
        /// </summary>
        /// <param name="deposit">Initialized WithdrawTransaction object, to be used to execute withdrawal</param>
        public void ExecuteTransaction(WithdrawTransaction withdraw)
        {
            withdraw.Execute();
        }

        /// <summary>
        /// Calls the Execute method to transfer funds, using the TransferTransaction object.
        /// </summary>
        /// <param name="deposit">Initialized TransferTransaction object, to be used to execute transfer</param>
        public void ExecuteTransaction(TransferTransaction transfer)
        {
            transfer.Execute();
        }
    }
}
