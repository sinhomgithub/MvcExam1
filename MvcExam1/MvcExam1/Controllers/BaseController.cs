using MvcExam1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcExam1.Controllers
{

    [ComputeExecuteTime]
    public class BaseController : Controller
    {

        protected 客戶資料Repository repo客戶資料 = RepositoryHelper.Get客戶資料Repository();
        protected 客戶銀行資訊Repository repo客戶銀行資訊 = RepositoryHelper.Get客戶銀行資訊Repository();
        protected 客戶聯絡人Repository repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository();        
        protected 客戶總合資訊Repository repo客戶總合資訊 = RepositoryHelper.Get客戶總合資訊Repository();
        protected 客戶分類清單Repository repo客戶分類清單 = RepositoryHelper.Get客戶分類清單Repository();



    }
}