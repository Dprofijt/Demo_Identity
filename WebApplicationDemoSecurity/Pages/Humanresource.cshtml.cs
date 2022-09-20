using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplicationDemoSecurity.Pages
{
    [Authorize(policy: "MustBelongToHRDepartment")]
    public class HumanresourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
