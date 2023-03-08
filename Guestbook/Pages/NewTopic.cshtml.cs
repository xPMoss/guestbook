using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guestbook
{
    public class NewTopicModel : PageModel
    {
        string topicTemp = null;
        string postTemp = null;

        public void OnGet()
        {
            Handler.alert = false;
            Handler.alertString = null;
        }

        public void OnPostTopic(string topic, string post)
        {
            topicTemp = topic;
            postTemp = post;

            Handler.alert = false;
            Handler.alertString = null;

            if(!String.IsNullOrEmpty(topicTemp) && !String.IsNullOrEmpty(postTemp) && !String.IsNullOrEmpty(Handler.CurrUser)){
                Handler.writeDBTopic(topicTemp, postTemp);
                Response.Redirect("Topics");
            }
            else{
                if(String.IsNullOrEmpty(topicTemp)){
                    Console.WriteLine("Topic is empty!!!");
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString += "Topic";       
                }

                if(String.IsNullOrEmpty(postTemp)){
                    Console.WriteLine("Post is empty!!!");    
                    if (!String.IsNullOrEmpty(Handler.alertString))
                    {
                        Handler.alertString += ", ";
                    }
                    Handler.alert = true;
                    Handler.alertString += "Post";       
                }

                if(String.IsNullOrEmpty(Handler.CurrUser)){
                    Console.WriteLine("You are not logged in!!!");      
                    Response.Redirect("Index");
                }
                              
                if (Handler.alert)
                {
                    Handler.alertString += ", is empty!!!"; 
                }

            }

            topicTemp = null;
            postTemp = null;
            //Console.WriteLine("New Topic: " + topicTemp + ", " + postTemp);

        }

        
    }
}
