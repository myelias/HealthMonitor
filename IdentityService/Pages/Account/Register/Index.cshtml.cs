using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;


namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    // The above two lines allow for an anonymous user who is 
    // unregistered to select the hyperlink
    public class Index : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}