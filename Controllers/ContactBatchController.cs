using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadExcelFile.Models;

namespace UploadExcelFile.Controllers
{
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
        public ActionResult EditById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.EditContactByBatchId(id);
            Session["BatchId"] = ContactBatchDB.EditContactByBatchId(id);
            return View(contactVM);
        }
    }
}