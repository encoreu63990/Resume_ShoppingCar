using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCar.Models
{
    public class Recipient
    {
        [DisplayName("收件人姓名")]
        public string Name { get; set; }

        [DisplayName("收件人信箱")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("收件人地址")]
        public string Address { get; set; }
    }
}