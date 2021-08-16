using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ShoppingCar.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DisplayName("訂單編號")]
        public string OrderGuid { get; set; }

        public int CustomerId { get; set; }

        [DisplayName("收件人姓名")]
        public string RecipientName { get; set; }

        [DisplayName("收件人地址")]
        public string RecipientEmail { get; set; }

        [DisplayName("收件人地址")]
        public string RecipientAddress { get; set; }

        [DisplayName("訂單日期")]
        public DateTime Date { get; set; }
    }
}