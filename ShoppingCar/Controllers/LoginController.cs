using ShoppingCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using ShoppingCar.ViewModels;

namespace ShoppingCar.Controllers
{
    public class LoginController : Controller
    {
        private static string constr => System.Configuration.ConfigurationManager.ConnectionStrings["ShoppingCarDatabase"].ConnectionString;

        public ActionResult Index()
        {
            if (LoginInfo.Customer != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel customer)
        {
            if (!ModelState.IsValid)
                return View();

            Customer customerInDb = null;
            using (var connection = new SqlConnection(constr))
            {
                string command =
                    @"SELECT * 
                        FROM [Customer] 
                       WHERE [Account] = @Account 
                             AND [Password] = @Password ";
                customerInDb = connection.QueryFirstOrDefault<Customer>(command, customer);
            }

            if (customerInDb == null)
            {
                ModelState.AddModelError("Customer", "登入失敗");
                return View();
            }

            LoginInfo.Customer = customerInDb;
            return RedirectToAction("Index", "Home");
        }
    }
}