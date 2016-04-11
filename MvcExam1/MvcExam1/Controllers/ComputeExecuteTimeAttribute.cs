using System;
using System.Web.Mvc;

namespace MvcExam1.Controllers
{
    public class ComputeExecuteTimeAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 儲存 Action 開始時間
            filterContext.Controller.ViewBag.ActionStartTime = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            DateTime endTime = DateTime.Now;
            filterContext.Controller.ViewBag.ActionEndTime = endTime;
            if(filterContext.Controller.ViewBag.ActionStartTime != null)
            {
                // 計算經過時間
                DateTime startTime = (DateTime)filterContext.Controller.ViewBag.ActionStartTime;
                TimeSpan ts = endTime - startTime;
                filterContext.Controller.ViewBag.ActionExecuteTimeSpan = ts;

                // 輸出執行時間到 Debug 視窗
                System.Diagnostics.Debug.WriteLine("Action [{0}/{1}] execute : spent {2:N3} sec.", 
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                    filterContext.ActionDescriptor.ActionName, 
                    ts.TotalSeconds);
            }

            base.OnActionExecuted(filterContext);
        }

    }
}