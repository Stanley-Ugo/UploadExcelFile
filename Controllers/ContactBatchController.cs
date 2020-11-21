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
            return View(ContactBatchDB.GetContactByBatchId(id));
        }
    }
}