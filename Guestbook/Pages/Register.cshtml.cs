using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    public class RegisterModel : PageModel
    {
        bool exist = false;

        public void OnGet()
        {
            Handler.alert = false;
            Handler.alertString = null;

        }

        public void OnGetExist(){
            Handler.alert = false;
            Handler.alertString = null;

            Console.WriteLine("Username already exist!!!");    
            Handler.alert = true;
            Handler.alertString = "Username already exist!!!";

        }

        public void OnPostRegister(string userid, string password, string name, string email)
        {
            Handler.alert = false;
            Handler.alertString = null;
            exist = Handler.checkDBUserExist(userid);

            if(!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(userid) && !String.IsNullOrEmpty(password)){
                
                if (!exist)
                {
                    Handler.writeDBUser(userid, password, name, email);
                    Handler.CurrUser = userid;
                    Handler.loggedIn = true;
                    Handler.alert = false;
                    Handler.alertString = null;
                    Response.Redirect("Topics");
                    Console.WriteLine("New User: " + userid + ", " + password + ", " + name + ", " + email);
                }

                
            }
            else {
                if(String.IsNullOrEmpty(name)){
                    Console.WriteLine("Name is empty!!!");    
                    Handler.alert = true;
                    Handler.alertString += "Name";
                }

                if(String.IsNullOrEmpty(email)){
                    Console.WriteLine("Email is empty!!!");
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString += "Email"; 
                }

                if(String.IsNullOrEmpty(userid)){
                    Console.WriteLine("Userid is empty!!!");
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString += "Username";   
                    
                }

                if(String.IsNullOrEmpty(password)){
                    Console.WriteLine("Password is empty!!!");  
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString += "Password";     
                }

                if (Handler.alert)
                {
                    Handler.alertString += ", is empty!!!"; 
                }
                

            }

            if(exist && !String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(userid) && !String.IsNullOrEmpty(password))
            {
                 Response.Redirect("Register?handler=exist");
            }


        }
    }
}
