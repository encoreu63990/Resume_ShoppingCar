using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCar.ViewModels
{
    public class RegisterVideModel
    {
        [DisplayName("帳號")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }

        [DisplayName("姓名")]
        [Required]
        public string Name { get; set; }

        [DisplayName("信箱")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}