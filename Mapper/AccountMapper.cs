using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dto;
using webapi.Model;

namespace webapi.Mapper
{
    public static class AccountMapper
    {
        public static AccountDto MapToAccountDto(this Account account) {
            
            return new AccountDto {
                UserName = account.UserName,
                Email = account.Email,
                Password = account.Password,
                Role = account.Role,
            };
        }

         public static AccountsDto MapToAccountsDto(this Account account) {
            
            return new AccountsDto {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt,
                UpdateAt = account.UpdateAt ?? null,
            };
        }

        public static Account MapToModel(this RegisterDto registerDto) {
            
            return new Account {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Role = registerDto.Role,
            };
        }
    }
}