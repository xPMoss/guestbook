using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            Console.WriteLine("Logging out!");
            Handler.logout();
            Response.Redirect("Index");
            

        }






    }


    
}
