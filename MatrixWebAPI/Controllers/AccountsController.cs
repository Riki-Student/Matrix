using MatrixWebAPI.Models;
using MatrixWebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MatrixWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsManagementContext _accountsManagementContext;
        public AccountsController(AccountsManagementContext accountsManagementContext)
        {
            _accountsManagementContext = accountsManagementContext;
        }

        [HttpGet]
        public ICollection<AccountBasicDto> Get()
        {          
            return _accountsManagementContext.Accounts.Select(account=>new AccountBasicDto { Id = account.Id, CompanyName=account.CompanyName, Website=account.Website }).ToList();
        }

        [HttpGet("{id}")]
        public Account Get(int id)
        {
            return _accountsManagementContext.Accounts.Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public IActionResult Post([FromBody] AccountBasicDto accountBasic)
        {
            try
            {
                var account = new Account
                {
                    CompanyName = accountBasic.CompanyName,
                    Website = accountBasic.Website
                };

                _accountsManagementContext.Accounts.Add(account);
                _accountsManagementContext.SaveChanges();

                return Ok(account);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AccountBasicDto accountBasic)
        {
            try
            {
                var account = _accountsManagementContext.Accounts.FirstOrDefault(x=>x.Id == id);
                if (account == null) return NotFound();

                account.CompanyName = accountBasic.CompanyName;
                account.Website = accountBasic.Website;
                
                _accountsManagementContext.SaveChanges();
                return Ok(accountBasic);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var account = _accountsManagementContext.Accounts.FirstOrDefault(x => x.Id == id);
                if (account == null) return NotFound();

                _accountsManagementContext.Accounts.Remove(account);
                _accountsManagementContext.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
