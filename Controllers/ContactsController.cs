using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View(new List<ContactVM>());
        }

        //POST: Contacts
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<ContactVM> contact = new List<ContactVM>();
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
                            ContactVM contactVM = new ContactVM();

                            contactVM.FirstName = row.Split(',')[0];
                            contactVM.LastName = row.Split(',')[1];
                            contactVM.Email = row.Split(',')[2];
                            contactVM.Telephone = row.Split(',')[3];
                            contactVM.Mobile = row.Split(',')[4];
                            contactVM.CompanyID = Convert.ToInt32(row.Split(',')[5]);

                            //Checking then first Name field
                            if (contactVM.FirstName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "First Name field can not be empty";
                                //TempData["Message"] = "First Name Field can not be Empty";
                                //return View("Error", TempData["Message4"] = "First Name can not be null!! ");
                            }

                            //checking the Last Name field
                            if (contactVM.LastName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Last Name field can not be empty";
                                //TempData["Message"] = "Last Name Field can not be Empty";
                                //return View("Error", TempData["Message4"] = "Last Name can not be null!!");
                            }
                            
                            //checking for email field
                            if (contactVM.Email == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Email field can not be empty";
                                //TempData["Message"] = "Email Field can not be Empty";
                                //return View("Error", TempData["Message4"] = "Email Name can not be null!!");
                            }

                            //checking for Mobile
                            if (contactVM.Mobile == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Mobile field can not be empty";
                                //TempData["Message"] = "Mobile Field can not be Empty";
                                //return View("Error", TempData["Message4"] = "Mobile can not be null!!");
                            }

                            //checking for Valid Company Id
                            if (contactVM.CompanyID != 7)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Invalid Company iD";
                                //TempData["Message"] = "Invalid Company Id.";
                                //return View("Error", TempData["Message4"] = "Invalid Company iD!!! ");
                            }

                            contact.Add(contactVM);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "No File Chosen";
                    TempData["Message"] = "Something went wrong " + ex.Message;
                }
            }
           //Seession object
   
            return View(contact);
        }
    }
}