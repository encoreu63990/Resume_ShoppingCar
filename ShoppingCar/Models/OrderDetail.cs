using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ShoppingCar.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public string OrderGuid { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        [DisplayName("產品編號")]
        public string ProductName { get; set; }

        [DisplayName("單價")]
        public int Price { get; set; }

        [DisplayName("訂購數量")]
        public int Quantity { get; set; }

    }
}