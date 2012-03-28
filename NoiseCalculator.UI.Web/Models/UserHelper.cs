using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiseCalculator.UI.Web.Models
{
    public class UserHelper
    {
        public static string GetUsernameWithoutDomain(string username)
        {
            return username.Split('\\')[1].ToUpper();
        }
    }
}