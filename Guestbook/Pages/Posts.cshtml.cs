using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    
    public class PostsModel : PageModel
    {

        string postTemp = null;

        public void OnGet()
        {
            Handler.alert = false;
            Handler.alertString = null;
            
        }

        public void OnPostPost(string post)
        {
            postTemp = post;
            Handler.alert = false;
            Handler.alertString = null;

            if(!String.IsNullOrEmpty(Handler.CurrTopic) && !String.IsNullOrEmpty(postTemp) && !String.IsNullOrEmpty(Handler.CurrUser)){
                Handler.writeDBPost(Handler.CurrTopic, postTemp);
                Console.WriteLine("New Post: " + postTemp);
            }
            else{
                if(String.IsNullOrEmpty(postTemp)){
                    Console.WriteLine("Post is empty!!!");    
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString = "Post is empty!!!";       
                }

                if(String.IsNullOrEmpty(Handler.CurrUser)){
                    Console.WriteLine("You are not logged in!!!");      
                    Response.Redirect("Index");
                }
            }

            postTemp = null;
            
        }




    }
}
