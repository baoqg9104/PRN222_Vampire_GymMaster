using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MSSQLServer.EntitiesModels;
using Services.Services;
using System.Security.Claims;

namespace GymMaster_RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User Account { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if(User.IsInRole("Trainer"))  return RedirectToPage("/Dashboard/TrainerDasboard");    
                return RedirectToPage("/Dashboard/MemberDashboard");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (User.IsInRole("Trainer")) return RedirectToPage("/Dashboard/TrainerDasboard");

                return RedirectToPage("/Dashboard/MemberDashboard");
            }


            var userAccount = _userService.Login(Account.Email,  Account.PasswordHash);
            //if (lionAccount == null)
            //{
            //    Message = "User does not exist or password is incorrect!";
            //    ModelState.AddModelError(string.Empty, Message);

            //    return Page();
            //}
            if (userAccount != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userAccount.UserId.ToString()),
                         new Claim(ClaimTypes.Email, userAccount.Email),
                        new Claim(ClaimTypes.Role, userAccount.Role.ToString())
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            if (userAccount.Role == "Trainer") return RedirectToPage("/Dashboard/TrainerDasboard");

                

                return RedirectToPage("/Dashboard/MemberDashboard");
            }

            Message = "User does not exist or password is incorrect.";
            return Page();
        }
    }
}
