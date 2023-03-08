using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    public class MySiteModel : PageModel
    {
        public void OnGet()
        {

            Console.WriteLine("Mysite Login: " + Handler.loggedIn);
            
        }

        public void OnPostChange(string topic, string post)
        {
            Response.Redirect("WIP");


        }





    }


    
}
