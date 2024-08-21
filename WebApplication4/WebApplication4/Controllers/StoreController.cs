using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class StoreController : Controller
    {
        private string connectionString = "Data Source=.;Initial Catalog=BikeStore;TrustServerCertificate=True;Integrated Security=SSPI";
        public IActionResult Index(string filter)

        {
            if (!string.IsNullOrEmpty(filter))
            {
                if(int.TryParse(filter, out int id)) { }
            }
            var staffs = new List<Staff>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                string query;
                
                    


                
                
               
                     query = "SELECT STAFF.staff_id,STAFF.first_name,STAFF.last_name,STAFF.email,STAFF.phone,STAFF.store_id,store_name,MANAGER.first_name AS [manager first name],MANAGER.last_name AS  [manager last name] FROM sales.staffs AS STAFF  INNER JOIN sales.stores AS STORES ON STAFF.store_id = STORES.store_id LEFT JOIN sales.staffs MANAGER ON  STAFF.manager_id = MANAGER.staff_id;";
                    cmd.CommandText = query;
               
                
                cmd.Connection = conn;

                //SqlCommand cmd = new SqlCommand(commandText, conn);
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
        public IActionResult Filter(searchStaff searchStaff)
        {
            //if(searchStaff == null)
            //if (string.IsNullOrEmpty(storeID) || !int.TryParse(storeID, out _))
            //{
            //    return RedirectToAction("Index");
            //}
            //var query = @"SELECT STAFF.staff_id,STAFF.first_name,STAFF.last_name,STAFF.email,STAFF.phone,STAFF.store_id,store_name
            //            ,MANAGER.first_name AS [manager first name],MANAGER.last_name AS  [manager last name] FROM sales.staffs AS STAFF
            //            INNER JOIN sales.stores AS STORES ON STAFF.store_id = STORES.store_id
            //            LEFT JOIN sales.staffs MANAGER ON  STAFF.manager_id = MANAGER.staff_id
            //            WHERE STAFF.store_id = @storeID;";
            //var staffs = new List<Staff>();
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Add("@storeID", SqlDbType.Int).Value = Convert.ToInt32(storeID);
            //    conn.Open();
               
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var staff = new Staff();
            //        staff.Id = Convert.ToInt32(reader["staff_id"]);
            //        staff.FirstName = reader["first_name"].ToString();
            //        staff.LastName = reader["last_name"].ToString();
            //        staff.Email = reader["email"].ToString();
            //        staff.Phone = reader["phone"].ToString();
            //        staff.StoreId = Convert.ToInt32(reader["store_id"]);
            //        staff.StoreName = reader["store_name"].ToString();
            //        staff.ManagerFirstName = reader["manager first name"].ToString();
            //        staff.ManagerLastName = reader["manager last name"].ToString();
            //        staffs.Add(staff);
            //    }
            //    conn.Close();
            //}


            //return View("Index",staffs);
            return Json(searchStaff);
           


        }
        //public IActionResult FilterByStoreName(String storeName)
        //{
        //    return Json(new {  storeName });

        //}
    }
}
