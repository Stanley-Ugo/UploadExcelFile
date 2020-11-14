using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using UploadExcelFile.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace UploadExcelFile.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        [HttpGet]
        public ActionResult Index()
        {
            return View(new List<Contact>());
        }

        //POST: Contacts
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<Contact> contact = new List<Contact>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string firstName = row.Split(',')[0];
                            string lastName = row.Split(',')[1];
                            string email = row.Split(',')[2];
                            string telephone = row.Split(',')[3];
                            string mobile = row.Split(',')[4];
                            int companyId = Convert.ToInt32(row.Split(',')[5]);

                            //Checking then first Name field
                            if (firstName == string.Empty)
                            {
                                TempData["Message"] = "First Name Field can not be Empty";
                                return View("Error", TempData["Message4"] = "First Name can not be null!! ");
                            }

                            //checking the Last Name field
                            if (lastName == string.Empty)
                            {
                                TempData["Message"] = "Last Name Field can not be Empty";
                                return View("Error", TempData["Message4"] = "Last Name can not be null!!");
                            }
                            

                            //checking for email field
                            if (email == string.Empty)
                            {
                                TempData["Message"] = "Email Field can not be Empty";
                                return View("Error", TempData["Message4"] = "Email Name can not be null!!");
                            }

                            //checking for Mobile
                            if (mobile == string.Empty)
                            {
                                TempData["Message"] = "Mobile Field can not be Empty";
                                return View("Error", TempData["Message4"] = "Mobile can not be null!!");
                            }

                            //checking for Valid Company Id
                            if (companyId != 7)
                            {
                                TempData["Message"] = "Invalid Company Id.";
                                return View("Error", TempData["Message4"] = "Invalid Company iD!!! ");
                            }


                            contact.Add(new Contact
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                Telephone = telephone,
                                Mobile = mobile,
                                CompanyID = companyId


                            });

                            //checking for nulls befor posting to Database
                            if (firstName != string.Empty && lastName != string.Empty && email != string.Empty && mobile != string.Empty && companyId == 7)
                            {
                                this.PostToDatabase(firstName, lastName, email, telephone, mobile, companyId);
                                TempData["Message2"] = "Upload Successful";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "No File Chosen";
                    TempData["Message"] = "Something went wrong " + ex.Message;
                }
            }
           

            return View(contact);
        }

        private void PostToDatabase(string firstName,
            string lastName, string email, string telephone, string mobile, int companyID)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("spCreateContact", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramFirstName = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    Value = firstName
                };
                cmd.Parameters.Add(paramFirstName);

                SqlParameter paramLastName = new SqlParameter
                {
                    ParameterName = "@LastName",
                    Value = lastName
                };
                cmd.Parameters.Add(paramLastName);

                SqlParameter paramEmail = new SqlParameter
                {
                    ParameterName = "@Email",
                    Value = email
                };
                cmd.Parameters.Add(paramEmail);

                SqlParameter paramTelephone = new SqlParameter
                {
                    ParameterName = "@Telephone",
                    Value = telephone
                };
                cmd.Parameters.Add(paramTelephone);

                SqlParameter paramMobile = new SqlParameter
                {
                    ParameterName = "@Mobile",
                    Value = mobile
                };
                cmd.Parameters.Add(paramMobile);

                SqlParameter paramCompanyID = new SqlParameter
                {
                    ParameterName = "@CompanyID",
                    Value = companyID
                };
                cmd.Parameters.Add(paramCompanyID);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }
    }
}