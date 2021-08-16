using ShoppingCar.Models;
using ShoppingCar.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using ShoppingCar.ViewModels;

namespace ShoppingCar.Controllers
{

    public class HomeController : Controller
    {
        private static string connectionString => System.Configuration.ConfigurationManager.ConnectionStrings["ShoppingCarDatabase"].ConnectionString;

        //[RequireLogin]
        public ActionResult Index()
        {
            IEnumerable<Product> products = null;
            using (var connection = new SqlConnection(connectionString))
            {
                string command =
                    @"SELECT * 
                        FROM [Product] ";
                products = connection.Query<Product>(command);
            }
            return View(products);
        }

        [RequireLogin]
        public ActionResult ShoppingCar()
        {
            var customerId = LoginInfo.Customer.Id;
            ShoppingCarViewModel viewModel = new ShoppingCarViewModel();
            using (var connection = new SqlConnection(connectionString))
            {
                var command =
                    @"SELECT * 
                        FROM [OrderDetail] 
                       WHERE [CustomerId] = @CustomerId 
                             AND [OrderGuid] IS NULL 
                       ORDER BY [ProductId] ";
                viewModel.OrderDetails = connection.Query<OrderDetail>(command, new { CustomerId = customerId });
            }
            return View(viewModel);
        }

        [RequireLogin]
        [HttpPost]
        public ActionResult ShoppingCar(Recipient recipient)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("ShoppingCar");

            var customerId = LoginInfo.Customer.Id;
            var orderGuid = Guid.NewGuid().ToString();

            var order = new Order()
            {
                OrderGuid = orderGuid,
                CustomerId = customerId,
                RecipientName = recipient.Name,
                RecipientEmail = recipient.Email,
                RecipientAddress = recipient.Address,
                Date = DateTime.Now
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateOrderDetailCommand =
                             @"UPDATE [OrderDetail] 
                                 SET [OrderGuid] = @OrderGuid 
                               WHERE [CustomerId] = @CustomerId 
                                     AND [OrderGuid] IS NULL ";
                        var productsInTheOrder = connection.Execute(updateOrderDetailCommand, new { customerId, orderGuid }, transaction);
                        if (productsInTheOrder == 0)
                        {
                            return RedirectToAction("ShoppingCar");
                        }

                        var insertOrderCommand =
                            @"INSERT [Order] 
                                     (OrderGuid, CustomerId, RecipientName, RecipientEmail, RecipientAddress, Date) 
                              VALUES (@OrderGuid, @CustomerId, @RecipientName, @RecipientEmail, @RecipientAddress, @Date) 
                              SELECT CAST(SCOPE_IDENTITY() as int)";
                        var orderId = connection.ExecuteScalar<int>(insertOrderCommand, order, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return RedirectToAction("OrderList");
        }

        [RequireLogin]
        public ActionResult OrderList()
        {
            var customerId = LoginInfo.Customer.Id;
            IEnumerable<Order> orders = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var command =
                    @"SELECT * 
                        FROM [Order] 
                       WHERE [CustomerId] = @CustomerId ";
                orders = connection.Query<Order>(command, new { customerId });
            }
            return View(orders);
        }

        [RequireLogin]
        public ActionResult OrderDetail(string orderGuid)
        {
            var customerId = LoginInfo.Customer.Id;
            IEnumerable<OrderDetail> orderDetails = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var command =
                    @"SELECT * 
                        FROM [OrderDetail] 
                       WHERE [CustomerId] = @CustomerId 
                             AND [OrderGuid] = @OrderGuid 
                       ORDER BY [ProductId] ";
                orderDetails = connection.Query<OrderDetail>(command, new { customerId, orderGuid });
            }

            if (!orderDetails.Any())
                return RedirectToAction("OrderList");

            return View(orderDetails);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        #region ajax request 
        [RequireLogin]
        public void AddProduct(int productId)
        {
            var customerId = LoginInfo.Customer.Id;
            using (var connection = new SqlConnection(connectionString))
            {
                var command =
                    @"UPDATE [OrderDetail] 
                         SET [Quantity] = Quantity + 1 
                       WHERE [ProductId] = @ProductId 
                             AND [CustomerId] = @CustomerId 
                             AND [OrderGuid] IS NULL ";
                int affectRows = connection.Execute(command, new { customerId, productId });

                // 沒有任何資料被更新
                if (affectRows == 0)
                {
                    var insertCommand =
                        @"INSERT [OrderDetail] 
                                 (CustomerId, ProductId, ProductName, Price, Quantity) 
                          SELECT @CustomerId AS CustomerId
                                 , @ProductId AS ProductId
                                 , Name AS ProductName
                                 , Price, 1 AS Quantity
                            FROM [Product] 
                           WHERE [Id] = @ProductId ";
                    connection.Execute(insertCommand, new { customerId, productId });
                }
            }
        }

        [RequireLogin]
        public void RemoveShoppingCarProduct(int orderDetailId)
        {
            var customerId = LoginInfo.Customer.Id;
            using (var connection = new SqlConnection(connectionString))
            {
                string command =
                    @"DELETE [OrderDetail] 
                       WHERE [CustomerId] = @CustomerId 
                             AND [Id] = @Id 
                             AND [OrderGuid] IS NULL ";
                connection.Execute(command, new { CustomerId = customerId, Id = orderDetailId });
            }
        }
        #endregion
    }
}