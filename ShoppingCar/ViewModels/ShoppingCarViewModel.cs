using ShoppingCar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ShoppingCar.ViewModels
{
    public class ShoppingCarViewModel
    {
        public ShoppingCarViewModel()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public Recipient Recipient { get; set; }
    }
}