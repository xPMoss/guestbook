using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            Handler.alert = false;
            Handler.alertString = null;
            Console.WriteLine("Login: " + Handler.loggedIn);

        }


        public void OnPostLogin(string userid, string password)
        {
            Handler.alert = false;
            Handler.alertString = null;
            
            Console.WriteLine("Trying to login, User: " + userid + ", Password: " + password);

            if (Handler.login(userid, password))
            {
                Handler.CurrUser = userid;
                Handler.loggedIn = true;
                Response.Redirect("Topics");
                
            }
            else{
                Console.WriteLine("Email is empty!!!");
                Handler.alert = true;
                Handler.alertString = "Wrong username and/or password"; 
            }


        }



    }


    
}
