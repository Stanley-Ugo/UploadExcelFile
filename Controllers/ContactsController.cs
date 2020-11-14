using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using UploadExcelFile.Models;

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
                                TempData["Message"] = "First Name Field can not be Empty";

                            //checking the Last Name field
                            if (lastName == string.Empty)
                                TempData["Message"] = "Last Name Field can not be Empty";
                            

                            //checking for email field
                            if (email == string.Empty)
                                TempData["Message"] = "Email Field can not be Empty";

                            //checking for Mobile
                            if (mobile == string.Empty)
                                TempData["Message"] = "Mobile Field can not be Empty";

                            //checking for Valid Company Id
                            if (companyId != 7)
                                TempData["Message"] = "Invalid COmpany ID";


                            contact.Add(new Contact
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                Telephone = telephone,
                                Mobile = mobile,
                                CompanyID = companyId


                            });

                            this.PostToDatabase(contact);
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

        private void PostToDatabase(List<Contact> contact)
        {
            //ADO>NET CODE connection string
            

        }
    }
}