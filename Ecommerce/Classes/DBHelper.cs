using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Classes
{
    public class DBHelper
    {
        public static Response SaveChanges(EcommerceContext db) {
            try
            {
                db.SaveChanges();
                return new Response { Succeeded = true };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "There is a record with a same value";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "the record can't delete because it has releted records";
                }
                else {
                    response.Message = ex.Message.ToString();

                }
                return response;
            }

        }


        public static int GetState(string description, EcommerceContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state == null) {
                state = new State { Description = description };
                db.States.Add(state);
                db.SaveChanges();
            }
            return state.StateId;
        }
    }
}