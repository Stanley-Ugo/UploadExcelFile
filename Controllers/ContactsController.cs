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
                    //Checking for the first Name field
                    if (string.IsNullOrEmpty(row.Split(',')[0]))
                    {
                        TempData["Message"] = "First Name Field is Missing";
                    }

                    //checking for Last Name Field
                    if (string.IsNullOrEmpty(row.Split(',')[1]))
                    {
                        TempData["Message"] = "Last Name Field is Missing";
                    }

                    //Checking for Email Field
                    if (string.IsNullOrEmpty(row.Split(',')[2]))
                    {
                        TempData["Message"] = "Email Field is Missing";
                    }

                    //checking for Mobile field
                    if (string.IsNullOrEmpty(row.Split(',')[0]))
                    {
                        TempData["Message"] = "Mobile Field is Missing";
                    }

                    //Checking for a valid companyID
                    int companyid = Convert.ToInt32(row.Split(',')[5]);
                    if (companyid != 7 )
                    {
                        TempData["Message"] = "Invalid Company ID";
                    }

                    if (!string.IsNullOrEmpty(row))
                    {

                        contact.Add(new Contact
                        {
                            FirstName = row.Split(',')[0], //Convert.ToInt32(row.Split(',')[0]),
                            LastName = row.Split(',')[1],
                            Email = row.Split(',')[2],
                            Telephone = row.Split(',')[3],
                            Mobile = row.Split(',')[4],
                            CompanyID = Convert.ToInt32(row.Split(',')[5])
                        });
                    }
                }
                

            }
            else
            {
                TempData["Message"] = "No File Chosen";
            }

            return View(contact);
        }
    }
}