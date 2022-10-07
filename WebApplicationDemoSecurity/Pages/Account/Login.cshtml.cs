using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplicationDemoSecurity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty] public Credential Credential { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Validate Credential
            if (Credential.UserName == "admin" && Credential.Password == "password")
            {
                // 4. Creating the security context for this user and give him his claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, " admin "),
                    new Claim(ClaimTypes.Email, " admin@mywebsite.com "),
                    new Claim("Department", "HR"), 
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    // 8a. add claim for custom policy
                    new Claim("EmploymentDate", "2021-10-01")
                };
                // 5. Create the identity for this user
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                // 6. Will set the cookie in the browser
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
