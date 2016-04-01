using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcExam1.Models
{
    public class 手機驗證格式Attribute : DataTypeAttribute
    {
        public 手機驗證格式Attribute() : base(DataType.Text)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null || value.GetType() != typeof(string))
                return false;

            string str = (string)value;            

            Regex rx = new Regex(@"\d{4}-\d{6}");
            bool b = rx.Match(str).Success;

            return b;
        }

    }
}