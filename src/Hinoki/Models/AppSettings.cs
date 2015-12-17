using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Hinoki.Models
{
    public class AppSettings
    {
        public static string GetDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }

        public static string GetBlogUrl()
        {
            return ConfigurationManager.AppSettings["BlogUrl"];
        }

        public static string GetBlogSystemName()
        {
            return "log";
        }        
    }
}