using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCar.Models
{
    /// <summary>
    /// 顧客資訊
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }

        [DisplayName("帳號")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required]
        public string Password { get; set; }

        [DisplayName("姓名")]
        [Required]
        public string Name { get; set; }

        [DisplayName("信箱")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}