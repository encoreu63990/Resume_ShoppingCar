using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ShoppingCar.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("產品名稱")]
        public string Name { get; set; }

        [DisplayName("單價")]
        public int Price { get; set; }

        [DisplayName("圖示")]
        public string Image { get; set; }
    }
}