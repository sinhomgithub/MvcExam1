using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcExam1.Library
{
    public class AppHelper
    {

        /// <summary>
        /// 取得所有 Exception 的資訊
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionString(Exception ex)
        {
            string str = "";
            while(ex != null)
            {
                str += String.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                ex = ex.InnerException;
            }

            return str;
        }

    }
}