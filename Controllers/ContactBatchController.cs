using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.SessionState;
using UploadExcelFile.Models;

namespace UploadExcelFile.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class ContactBatchController : Controller
    {
        // GET: ContactBatch
        public ActionResult Index()
        {
            List<ContactBatch> contactBatchDBs = new List<ContactBatch>();
            contactBatchDBs = ContactBatchDB.GetAllBatches();
            return View(contactBatchDBs);
        }

        [HttpGet]
        public ActionResult GetFileById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.GetContactByBatchId(id);
            return View(contactVM);
        }
        
        [HttpGet]
        public ActionResult DeleteById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.DeleteContactByBatchId(id);
            return View(contactVM);
        }

        [HttpGet]
        public ActionResult DeletedBatchById(int id)
        {
            ContactBatchDB.DeleteFileByBatchId(id);
            return View();
        }

        [HttpGet]
        [WebMethod(EnableSession = true)]
        public ActionResult EditById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.EditContactByBatchId(id);
            Session["BatchId"] = ContactBatchDB.EditContactByBatchId(id);
            return View(contactVM);
        }

        [HttpGet]
        public ActionResult ReUploadBatch()
        {
            return View(new List<ContactVM>());
        }

        [HttpPost]
        [WebMethod(EnableSession = true)]
        public ActionResult ReUploadBatch(HttpPostedFileBase postedFile)
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
            Session["ReUploadBatch"] = contact;

            return View(contact);
        }

        [HttpGet]
        [WebMethod(EnableSession = true)]
        public ActionResult UpdateByBatchId(ContactVM contactVM)
        {
            int batchID = 0;
            List<ContactVM> batchId = new List<ContactVM>();
            batchId = (List<ContactVM>)Session["BatchId"];
            foreach(ContactVM contactsBatch in batchId)
            {
                batchID = contactsBatch.BatchId;
            }
            
            //Session object
            List<ContactVM> contacts = new List<ContactVM>();
            contacts = (List<ContactVM>)Session["ReUploadBatch"];

            ContactBatchDB.UpdateContactByBatchId(contacts, batchID);

            return View();
        }
    }
}