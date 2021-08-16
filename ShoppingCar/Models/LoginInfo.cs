using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCar.Models
{
    public class LoginInfo
    {
        public static Customer Customer
        {
            get
            {
                return (Customer)HttpContext.Current.Session["Customer"];
            }
            set
            {
                HttpContext.Current.Session["Customer"] = value;
            }
        }
    }
}