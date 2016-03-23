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
    public class 客戶總合資訊Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶總合資訊
        public ActionResult Index(string keyword)
        {
            var data = db.客戶總合資訊.AsQueryable();

            // 如果使用搜尋
            if (String.IsNullOrWhiteSpace(Request["keyword"]) == false)
            {
                string keywordStr = Request["keyword"].Trim();
                data = data.Where(p => p.客戶名稱.IndexOf(keywordStr) != -1 );
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
            客戶總合資訊 客戶總合資訊 = db.客戶總合資訊.Find(id);
            if (客戶總合資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶總合資訊);
        }      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
