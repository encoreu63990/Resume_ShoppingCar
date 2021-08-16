using ShoppingCar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using ShoppingCar.Models;

namespace ShoppingCar.Controllers
{
    public class RegisterController : Controller
    {
        private static string constr => System.Configuration.ConfigurationManager.ConnectionStrings["ShoppingCarDatabase"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterVideModel customer)
        {
            if (!ModelState.IsValid)
                return View();

            using (var connection = new SqlConnection(constr))
            {
                string command =
                    @"SELECT COUNT(*) 
                        FROM [Customer] 
                       WHERE [Account] = @Account 
                             OR [Email] = @Email ";

                var customerExist = connection.ExecuteScalar<bool>(command, customer);
                if (customerExist)
                {
                    ModelState.AddModelError("Customer", "註冊失敗：會員已存在");
                    return View();
                }

                var insertCommand =
                    @"INSERT [Customer] 
                             (Account, Password, Name, Email) 
                      VALUES (@Account, @Password, @Name, @Email) 
                      SELECT CAST(SCOPE_IDENTITY() as int) ";
                var customerId = connection.ExecuteScalar<int>(insertCommand, customer);

                LoginInfo.Customer = new Customer
                {
                    Id = customerId,
                    Account = customer.Account,
                    Password = customer.Password,
                    Name = customer.Name,
                    Email = customer.Email
                };
            }

            return RedirectToAction("Index", "Home");
        }
    }
}