using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tracking_Tool_System.Pages
{
    [Authorize]
    public class IndexModel : PageModel 
    { 
        public void OnGet()
        { 

        } 
    }   
}
