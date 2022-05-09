using SampleMvcProject.Classes;
using SampleMvcProject.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleMvcProject.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        private const string cookieValue = "ILoveCookie";
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["sampleMvc"] != null)
            {
                var cookie = Request.Cookies["sampleMvc"];
                var cookieText = EncryptorDecryptor.Base64Decode(cookie.Value);
                if (cookieText.Contains(cookieValue))
                {
                    var username = cookieText.Split('-')[0];

                    if (!string.IsNullOrEmpty(username))
                    {
                        int userId = GetUserIdByUserName(username);
                        if (userId > 0)
                        {
                            Session["Name"] = username;
                            Session["Id"] = userId;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var name = form["username"];
            var password = form["password"];
            if (ValidateUser(name, password))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.InvalidUser = true;
            return View();
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_LOGIN";
                cmd.Parameters.AddWithValue("@USERNAME", username);
                cmd.Parameters.AddWithValue("@PASSWORD", EncryptorDecryptor.Base64Encode(password));
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Session["Name"] = dt.Rows[0]["Name"];
                    Session["Id"] = dt.Rows[0]["Id"];
                    var cookie = new HttpCookie("sampleMvc");
                    cookie.Value = EncryptorDecryptor
                        .Base64Encode($"{(string)dt.Rows[0]["Name"]}-{cookieValue}");
                    Response.Cookies.Add(cookie);
                    return true;
                }

            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private int GetUserIdByUserName(string username)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PROC_GET_USER_BY_USERNAME";
                cmd.Parameters.AddWithValue("@USERNAME", username);
                cmd.Connection = ClsConnections.Con();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return (int)dt.Rows[0]["Id"];
                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }
    }
}