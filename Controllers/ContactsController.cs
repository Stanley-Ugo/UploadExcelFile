﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadExcelFile.Models;
using System.Web.SessionState;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;

namespace UploadExcelFile.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class ContactsController : Controller
    {
        // GET: Contacts
        [HttpGet]
        public ActionResult Index()
        {
            Session.Clear();
            return View(new List<ContactVM>());
        }

        //POST: Contacts
        [HttpPost]
        [WebMethod(EnableSession = true)]
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
                                contactVM.Reason = "First Name field is required";
                            }

                            //checking the Last Name field
                            if (contactVM.LastName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Last Name field is required";
                            }
                            
                            //checking for email field
                            if (contactVM.Email == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Email field is required";
                            }

                            //checking for Mobile
                            if (contactVM.Mobile == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Mobile field is required";
                            }

                            //checking for Valid Company Id
                            if (contactVM.CompanyID != 7)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Reason = "Invalid Company iD";
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
            Session["Upload"] = contact;

            return View(contact);
        }

        //Post Method to the Database
        [HttpGet]
        [WebMethod(EnableSession = true)]
        public ActionResult ContactsTable()
        {
            DateTime myDateTime = DateTime.Now;
            string sqlformattedDate = myDateTime.ToString("yyyy-MM-dd hh:mm:ss.fff");
            string batchName = GetBatchName();
            //Creating a new Batch object
            ContactBatch batch = new ContactBatch();
            batch.BatchName = batchName;
            batch.CreatedBy = "System";
            batch.DateCreated = Convert.ToDateTime(sqlformattedDate);

            //calling the post to ContactBatch table--
            int batchID = ContactDB.GetBatchID(batch);

            List<ContactVM> contacts = new List<ContactVM>();
            contacts = (List<ContactVM>)Session["Upload"];
            ContactDB.PostToDatabase(contacts, batchID);
            return View(contacts);
        }

        private string GetBatchName()
        {
            string batchName;
            var rand = new Random();
            ContactBatch batch = new ContactBatch();
            var newRand = rand.Next(100);

            batchName = batch.BatchName = "Upload - " + newRand;

            return batchName;
        }
    }
}