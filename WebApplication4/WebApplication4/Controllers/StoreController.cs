using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class StoreController : Controller
    {
        private string connectionString = "Data Source=.;Initial Catalog=BikeStore;TrustServerCertificate=True;Integrated Security=SSPI";
        private dynamic GetOrdModel = new ExpandoObject();

        public IActionResult GetStaff()

        {

            var staffs = new List<Staff>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {



                var query = "SELECT STAFF.staff_id,STAFF.first_name,STAFF.last_name,STAFF.email,STAFF.phone,STAFF.store_id,store_name,MANAGER.first_name AS [manager first name],MANAGER.last_name AS  [manager last name] FROM sales.staffs AS STAFF  INNER JOIN sales.stores AS STORES ON STAFF.store_id = STORES.store_id LEFT JOIN sales.staffs MANAGER ON  STAFF.manager_id = MANAGER.staff_id;";


                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var staff = new Staff();
                    staff.Id = Convert.ToInt32(reader["staff_id"]);
                    staff.FirstName = reader["first_name"].ToString();
                    staff.LastName = reader["last_name"].ToString();
                    staff.Email = reader["email"].ToString();
                    staff.Phone = reader["phone"].ToString();
                    staff.StoreId = Convert.ToInt32(reader["store_id"]);
                    staff.StoreName = reader["store_name"].ToString();
                    staff.ManagerFirstName = reader["manager first name"].ToString();
                    staff.ManagerLastName = reader["manager last name"].ToString();
                    staffs.Add(staff);
                }
                conn.Close();
            }


            return View(staffs);
        }
        public IActionResult FilterStaff(string staffID, string storeName)
        {
            //if (searchStaff == null)
            //    if (string.IsNullOrEmpty(storeID) || !int.TryParse(storeID, out _))
            //    {
            //        return RedirectToAction("Index");
            //    }
            if (staffID == null && storeName == null)
            {
                return RedirectToAction("GetStaff");
            }
            var staffs = new List<Staff>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT STAFF.staff_id,STAFF.first_name,STAFF.last_name,STAFF.email,STAFF.phone,STAFF.store_id,store_name
                        ,MANAGER.first_name AS [manager first name],MANAGER.last_name AS  [manager last name] FROM sales.staffs AS STAFF
                        INNER JOIN sales.stores AS STORES ON STAFF.store_id = STORES.store_id
                        LEFT JOIN sales.staffs MANAGER ON  STAFF.manager_id = MANAGER.staff_id
                        ";
                SqlCommand cmd = new SqlCommand();
                if (staffID != null)
                {
                    query += "WHERE STAFF.staff_id = @staffID;";
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@staffID", SqlDbType.Int).Value = Convert.ToInt32(staffID);
                }
                else if (storeName != null)
                {

                    query += "WHERE store_name = @storeName;";
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@storeName", SqlDbType.VarChar).Value = storeName;

                }

                cmd.Connection = conn;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var staff = new Staff();
                    staff.Id = Convert.ToInt32(reader["staff_id"]);
                    staff.FirstName = reader["first_name"].ToString();
                    staff.LastName = reader["last_name"].ToString();
                    staff.Email = reader["email"].ToString();
                    staff.Phone = reader["phone"].ToString();
                    staff.StoreId = Convert.ToInt32(reader["store_id"]);
                    staff.StoreName = reader["store_name"].ToString();
                    staff.ManagerFirstName = reader["manager first name"].ToString();
                    staff.ManagerLastName = reader["manager last name"].ToString();
                    staffs.Add(staff);
                }
                conn.Close();
            }


            return View("GetStaff", staffs);




        }
        public IActionResult GetOrder(string orderId)
        {
            var orders = new List<Order>();
            if (!string.IsNullOrEmpty(orderId))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {



                    var query = @"SELECT order_id,C.first_name,C.last_name
                            , 'street: '+ street +'city: ' + city + 'state: ' + [state] AS [Address],
                            C.phone,order_date,required_date,shipped_date
                            ,S.first_name AS [staff first name],S.last_name AS [staff last name]
                            from sales.orders AS O 
                            INNER JOIN sales.customers AS C 
                            ON C.customer_id = O.customer_id
                            INNER JOIN sales.staffs AS S ON S.staff_id = O.staff_id
                            WHERE order_id = @orderId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var order = new Order();
                        order.Id = Convert.ToInt32(reader["order_id"]);
                        order.FirstName = reader["first_name"].ToString();
                        order.LastName = reader["last_name"].ToString();
                        order.Address = reader["Address"].ToString();
                        order.Phone = reader["phone"].ToString();

                        order.Date = DateTime.Parse(reader["order_date"].ToString());
                        order.RequiredDate = DateTime.Parse(reader["required_date"].ToString());
                        order.ShippingDate = DateTime.Parse(reader["shipped_date"].ToString());
                        order.StaffFirstName = reader["staff first name"].ToString();
                        order.StaffLastName = reader["staff last name"].ToString();
                        orders.Add(order);
                    }
                    conn.Close();
                }
            }

            GetOrdModel.Orders = orders;
            GetOrderDetails(orderId);
            
            return View(GetOrdModel);

        }
        private void GetOrderDetails(string orderId)
        {
            var orders = new List<OrderDetails>();
            decimal totalCost = 0;
            if (!string.IsNullOrEmpty(orderId))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {



                    var query = @"SELECT P.product_id,P.product_name,quantity,OI.list_price,discount FROM sales.order_items AS OI
                                INNER JOIN production.products AS P ON P.product_id = OI.product_id
                                WHERE order_id = @orderId;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var order = new OrderDetails(); 
                        order.ProductId = Convert.ToInt32(reader["product_id"]);
                        order.ProductName = reader["product_name"].ToString();
                        order.Quantity = Convert.ToInt32(reader["quantity"]);
                        order.ListPrice = Convert.ToDecimal(reader["list_price"].ToString());
                        order.Discount = Convert.ToDecimal(reader["discount"].ToString());

                        orders.Add(order);
                        totalCost += (1 - order.Discount) * order.ListPrice * order.Quantity;
                    }
                    conn.Close();
                }
            }

            GetOrdModel.Details = orders;
            GetOrdModel.totalCost = totalCost;



        }

    }
}
