using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;


namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    // The above two lines allow for an anonymous user who is 
    // unregistered to select the hyperlink
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]

        public RegisterViewModel Input {get; set;}

        [BindProperty]

        public bool RegisterSuccess {get; set;}
        public IActionResult OnGet(string returnUrl) // We are returning the page so we use IActionResult
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            };

            return Page(); // Return page to user
        }

        public async Task<IActionResult> OnPost() // Async Task that will return an IActionResult
        {
            if (Input.Button != "register") return Redirect("~/"); // Redirect back to home page

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser // Create new applciation user if form has all fields populated (Valid)
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password );

                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, Input.FullName)
                    });

                    RegisterSuccess = true;
                }
            }

            return Page();
        }
    }
}