using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Dto;
using webapi.Helper;
using webapi.Interface;
using webapi.Model;

namespace webapi.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context){
            _context = context;
        }
        public async Task<Account> RegisterAccount(Account account){
            
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            
            return account;
        }

        public async Task<Account?> DeleteAccount(int id){
            
            var account = await _context.Accounts.FindAsync(id);

            if (account == null) return null;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> GetAccountById(int id){
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<List<Account>> GetAllAccounts(QueryObject query){

            var accounts = _context.Accounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Keyword)){
                accounts = accounts.Where(a => a.UserName.Contains(query.Keyword) || a.Email.Contains(query.Keyword));
            }

            if (!string.IsNullOrWhiteSpace(query.Role)) {
                accounts = accounts.Where(a => a.Role.ToLower() == query.Role.ToLower());
            } 
    
            return await accounts.ToListAsync();
        }

        public async Task<Account?> UpdateAccount(int id, UpdateDto updateDto){

            var account = await _context.Accounts.FindAsync(id);

            if (account == null) return null;
            
            account.UserName = updateDto.UserName;
            account.Email = updateDto.Email;
            account.Password = updateDto.Password;
            account.Role = updateDto.Role;
            account.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return account;            
        }

        public async Task<bool> IsAccountExists(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> IsUsernameOrEmailExists(string email, string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email || a.UserName == username);
        }
    }
}