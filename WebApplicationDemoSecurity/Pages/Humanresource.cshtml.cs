using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplicationDemoSecurity.Pages
{
    // 7. Set the policy that is needed for this page (the policy is defined in Startup.cs)
    [Authorize(policy: "MustBelongToHRDepartment")]
    public class HumanresourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
