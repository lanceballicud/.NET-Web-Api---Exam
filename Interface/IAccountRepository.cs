using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dto;
using webapi.Helper;
using webapi.Model;

namespace webapi.Interface
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccounts(QueryObject query);
        Task<Account?> GetAccountById(int id);
        Task<Account> RegisterAccount(Account account);
        Task<Account?> UpdateAccount(int id, UpdateDto updateDto);
        Task<Account?> DeleteAccount(int id);
        Task<bool> IsAccountExists(int id);
        Task<bool> IsUsernameOrEmailExists(string email, string username);
    }
}