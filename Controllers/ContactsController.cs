﻿using Microsoft.AspNetCore.Http;
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
            }
        }
    }
}