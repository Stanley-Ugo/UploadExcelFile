using System;
using System.Collections.Generic;
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
        [WebMethod(EnableSession = true)]
        public ActionResult EditByBatchId(ContactVM contactVM)
        {
            ContactVM batchId = new ContactVM(); 
            batchId = (ContactVM)Session["BatchId"];
            int stronger = batchId.BatchId;
        }
    }
}