using MatrixWebAPI.Models.DTO;
using MatrixWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatrixWebAPI.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class UsersController : ControllerBase
    {
        private readonly AccountsManagementContext _accountsManagementContext;
        public UsersController(AccountsManagementContext accountsManagementContext)
        {
            _accountsManagementContext = accountsManagementContext;
        }

        [HttpGet("{accountId}/users")]
        public ICollection<User> Get(int accountId)
        {
            var users = _accountsManagementContext.Accounts
        .Where(account => account.Id == accountId)
        .SelectMany(account => account.AccountsUsers)
        .Select(accountsUser => accountsUser.User)
        .ToList();

    return users;
        }

        [HttpGet("{accountId}/users/{userId}")]
        public User Get(int accountId, int userId)
        {
            var user = _accountsManagementContext.AccountsUsers
        .Where(au => au.AccountId == accountId && au.UserId == userId)
        .Select(au => au.User)
        .FirstOrDefault();

            return user;
        }

        [HttpPost("{accountId}/users")]
        public IActionResult Post(int accountId, [FromBody] ICollection<UserBasicDto> users)
        {
            try
            {
                var account = _accountsManagementContext.Accounts.FirstOrDefault(x => x.Id == accountId);
                if (account == null) return NotFound();

                foreach (var user in users)
                {
                    var u = _accountsManagementContext.Users.FirstOrDefault(u => u.Email.Equals(user.Email));
                    if (u == null)
                    {
                        u = new User { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName };
                        _accountsManagementContext.Add(u);
                        _accountsManagementContext.SaveChanges();
                    }

                    _accountsManagementContext.AccountsUsers.Add(new AccountsUser { AccountId = accountId, UserId = u.Id });
                    _accountsManagementContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //account/23
        [HttpPut("{accountId}/users/{userId}")]
        public IActionResult Put(int accountId, int userId, [FromBody] UserBasicDto userBasic)
        {
            try
            {
                var user = _accountsManagementContext.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null) return NotFound();

                user.FirstName = userBasic.FirstName;
                user.LastName = userBasic.LastName;
                user.Email = userBasic.Email;//will automatically prevent a non unique value

                _accountsManagementContext.SaveChanges();
                return Ok(userBasic);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{accountId}/users/{userId}")]
        public IActionResult Delete(int accountId, int userId)
        {
            try
            {
                var account = _accountsManagementContext.Accounts.FirstOrDefault(x => x.Id == accountId);
                if (account == null) return NotFound();

                var user = _accountsManagementContext.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null) return NotFound();

                var accountsUser = _accountsManagementContext.AccountsUsers.FirstOrDefault(x => x.AccountId == accountId && x.UserId == userId);
                if (accountsUser == null) return NotFound();

                _accountsManagementContext.AccountsUsers.Remove(accountsUser);
                _accountsManagementContext.SaveChanges();

                if (!account.AccountsUsers.Any())
                {
                    // If no more users are associated with the account, delete the account
                    _accountsManagementContext.Accounts.Remove(account);
                    _accountsManagementContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
