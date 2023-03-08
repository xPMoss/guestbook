using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Guestbook
{
    
    public class TopicsModel : PageModel
    {

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }

        public void OnPostView(string topic)
        {
            Handler.CurrTopic = topic;
            Response.Redirect("Posts");
            Console.WriteLine("Current Topic: " + topic);
        }

    }
}
