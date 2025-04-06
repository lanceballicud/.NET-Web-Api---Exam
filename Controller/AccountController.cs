using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using webapi.Data;
using webapi.Dto;
using webapi.Helper;
using webapi.Interface;
using webapi.Mapper;
using webapi.Model;

namespace webapi.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController: ControllerBase
    {   
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepo;
        public AccountController(ApplicationDbContext context, IAccountRepository accountRepo)
        {
            _context = context;
            _accountRepo = accountRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
            var accounts = await _accountRepo.GetAllAccounts(query);
            
            var accountsDto = accounts.Select(account => account.MapToAccountsDto());

            return Ok(accountsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var account = await _accountRepo.GetAccountById(id);

            if (account == null) {
                return NotFound(new { message = "Account not found."});
            }
        
            var accountDto = account.MapToAccountDto();
            return Ok(accountDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {

            if (!ModelState.IsValid) {
                return BadRequest(new { message = "Invalid data." });
            }

            var account = registerDto.MapToModel();
            
            var existingAccount = await _accountRepo.IsUsernameOrEmailExists(account.Email, account.UserName);

            if (existingAccount) {
                return BadRequest(new { message= "Account with this username or email already exists" });
            }

            await _accountRepo.RegisterAccount(account);

            var accountDto = account.MapToAccountsDto();

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, new { message = "Account created successfully.", data = accountDto });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDto updateDto) {
            if (!ModelState.IsValid) {
                return BadRequest(new { message = "Invalid data." });
            }

            var accountToUpdate = await _accountRepo.UpdateAccount(id, updateDto!);

            if (accountToUpdate == null) {
                return BadRequest(new { message = "Email or password already exists."});
            }

            return Ok( new{ message = "Account updated successfully.", data = accountToUpdate.MapToAccountDto()});
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {

            var accountToDelete = await _accountRepo.DeleteAccount(id);

            if (accountToDelete == null) {
                return NotFound(new { message = "Account not found."});
            }

            return Ok(new { message = "Account deleted successfully.", data = accountToDelete.MapToAccountsDto() });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            if (!ModelState.IsValid) {
                return BadRequest(new { message = "Invalid data." });
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == loginDto.Email && a.Password == loginDto.Password);

            if (account == null) {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var accountDto = account.MapToAccountsDto();
            return Ok(new { message = "Login successful.", data = accountDto });
        }
    }
}