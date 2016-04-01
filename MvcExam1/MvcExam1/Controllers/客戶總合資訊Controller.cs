using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcExam1.Models;

namespace MvcExam1.Controllers
{
    public class 客戶總合資訊Controller : BaseController
    {
        
        // GET: 客戶總合資訊
        public ActionResult Index(string keyword, string export)
        {
            var data = repo客戶總合資訊.Query(keyword);

            // 如果使用搜尋
            if (String.IsNullOrEmpty(export) == false)
            {
                byte[] bs = repo客戶總合資訊.Export(data);
                return this.File(bs, "application/vnd.ms-excel", "客戶總合資訊.xls");
            }

            return View(data);
        }

        // GET: 客戶總合資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶總合資訊 客戶總合資訊 = repo客戶總合資訊.Find(id.Value);
            if (客戶總合資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶總合資訊);
        }      

       
    }
}
