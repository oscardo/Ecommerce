using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Classes
{
    public class MovementsHelper : IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public static Response NewOrder(NewOrderView view, string UserName)
        {
           
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
                    var viewOrder = new Order
                    {
                        CompanyID = user.CompanyID,
                        CustomerID = view.CustomerID,
                        Date = DateTime.Now,
                        Remarks = view.Remarks,
                        StateId = DBHelper.GetState("Created", db),
                    };
                    db.Orders.Add(viewOrder);
                    db.SaveChanges();

                    var detailsTotal = db.OrderDetailTmps.Where(odt => odt.UserName == UserName).ToList();

                    foreach (var details in detailsTotal)
                    {
                        var orderdetail = new OrderDetail
                        {
                            Description = details.Description,
                            OrderID = viewOrder.OrderID,
                            ProductID = details.ProductID,
                            Quantity = details.Quantity,
                            TaxRate = details.TaxRate,
                            Price = details.Price
                        };
                        db.OrderDetails.Add(orderdetail);
                        db.OrderDetailTmps.Remove(details);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response { Succeeded = false, Message = ex.Message.ToString()  };
                }
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
        
    }
}