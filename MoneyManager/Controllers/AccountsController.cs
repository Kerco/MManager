using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyManager.Data;
using MoneyManager.Models;

namespace MoneyManager.Controllers
{
    [Route("Accounts/[Action]")]
    public class AccountsController : Controller
    {

        private UserManager<Account> _userManager;
        private SignInManager<Account> _signInManager;
        private ILogger<AccountsController> _logger;
        private ManagerContext _context;

        public AccountsController(UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            ILogger<AccountsController> logger, ManagerContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> bevan()
        //{
        //    return Ok(HttpContext.User.Identity.IsAuthenticated);
        //}

        [HttpGet]
        public async Task<IActionResult> LoginClear()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> Login(string returnUrl = null)
        //{
        //    Clear the existing external cookie to ensure a clean login process
        //      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //    ViewData["ReturnUrl"] = returnUrl;
        //    string[] url = returnUrl.Split("/");
        //    RedirectToAction(url[2], url[1]);
        //    return Ok();

        //    return RedirectToAction(nameof(returnUrl));
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                //var result = await _signInManager.PasswordSignInAsync(model.Account, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return StatusCode(302);
                }
            }
            return StatusCode(302);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Account
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Gender = model.Gender,
                    BirthDay = model.BirthDay
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password");

                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return Ok();
                }
            }
            return BadRequest();
        }

        //    private readonly ManagerContext _context;

        //    public AccountsController(ManagerContext context)
        //    {
        //        _context = context;
        //    }

        [HttpGet]
        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }

        [HttpGet]
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == id.ToString());
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }


        [HttpGet]
        public async Task<IActionResult> DetailsByEmail(string email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var acc = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == email);
            if (acc == null)
            {
                return NotFound();
            }

            return Ok(acc);
        }


        [HttpGet]
        public async Task<IActionResult> DetailsByUserName(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var acc = await _context.Accounts.SingleOrDefaultAsync(a => a.UserName == username);
            if (acc == null)
            {
                return NotFound();
            }

            return Ok(acc);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string email)
        {
            if (email == null)
            {
                return BadRequest();
            }

            var acc = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == email);

            if (acc == null) { return NotFound(); }

            _context.Accounts.Remove(acc);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest();
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest();
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new Account { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return Ok();
                    }
                }
                return BadRequest();
            }
            return BadRequest();

        }


















        //    [HttpGet]
        //    public async Task<IActionResult> AccountWithEnvelopes(int? id)
        //    {
        //        if(id == null)
        //        {
        //            return NotFound();
        //        }

        //        var account = await _context.Accounts.Include(a => a.Envelopes).SingleOrDefaultAsync(m => m.Id == id);

        //        if(account == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(account);
        //    }


        //    // GET: Accounts/Create
        //    //public IActionResult Create()
        //    //{
        //    //    return View();
        //    //}

        //    // POST: Accounts/Create
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    //[ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([FromBody] Account account)
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            try
        //            {
        //                _context.Add(account);
        //                await _context.SaveChangesAsync();
        //                return Ok(account);

        //            }
        //            catch (Exception e)
        //            {
        //                Debug.WriteLine(e.Message);
        //                return StatusCode(409);
        //            }

        //        }
        //        return BadRequest();
        //    }

        //    // GET: Accounts/Edit/5
        //    //public async Task<IActionResult> Edit(int? id)
        //    //{
        //    //    if (id == null)
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
        //    //    if (account == null)
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    return View(account);
        //    //}

        //    // POST: Accounts/Edit/5
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost, ActionName("Edit")]
        //    // [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit([FromBody]Account account, int id)
        //    {
        //        if (id != account.Id)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(account);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!AccountExists(account.Id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    return StatusCode(409);
        //                }
        //            }
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //    //[HttpPut]
        //    //public async Task<IActionResult> Edit([FromBody] Account account, int id)
        //    //{
        //    //    if(account == null || account.Id != id)
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    var acc = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        //    //    if(acc == null)
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    acc.Envelopes = account.Envelopes;
        //    //    acc.Email = account.Email;
        //    //    acc.UserName = account.UserName;
        //    //    acc.Password = account.Password;
        //    //    acc.Gender = account.Gender;
        //    //    acc.BirthDay = account.BirthDay;

        //    //    _context.Update(acc);
        //    //    await _context.SaveChangesAsync();
        //    //    return new NoContentResult();
        //    //}

        //    // GET: Accounts/Delete/5
        //    //public async Task<IActionResult> Delete(int? id)
        //    //{
        //    //    if (id == null)
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    var account = await _context.Accounts
        //    //        .SingleOrDefaultAsync(m => m.Id == id);
        //    //    if (account == null)
        //    //    {
        //    //        return NotFound();
        //    //    }

        //    //    return View(account);
        //    //}

        //    // POST: Accounts/Delete/5
        //    [HttpDelete]
        //    //[ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        try
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }
        //            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);

        //            if (account == null)
        //            {
        //                NotFound();
        //            }

        //            _context.Accounts.Remove(account);
        //            await _context.SaveChangesAsync();
        //            return Ok();
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            Debug.WriteLine(ex.Message);
        //            return BadRequest();
        //        }

        //    }

        //    private bool AccountExists(int id)
        //    {
        //        return _context.Accounts.Any(e => e.Id == id);
        //    }
    }
}