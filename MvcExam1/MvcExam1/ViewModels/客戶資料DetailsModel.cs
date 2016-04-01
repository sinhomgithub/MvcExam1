using MvcExam1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcExam1.ViewModels
{
    public class 客戶資料BatchUpdateViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string 職稱 { get; set; }
        
        [Required]
        [StringLength(50)]
        [手機驗證格式(ErrorMessage = "手機格式無效, 請輸入, 如: 0911-123456 的內容")]
        public string 手機 { get; set; }

        [Required]
        [StringLength(50)]
        public string 電話 { get; set; }        

    }
}