using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcExam1.Models
{

    [MetadataType(typeof(客戶資料Metadata))]
    public partial class 客戶資料
    {
       
    }

    public class 客戶資料Metadata
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string 客戶名稱 { get; set; }

        [Required]        
        [StringLength(8, MinimumLength = 8, ErrorMessage = "統一編號長度必須為 8")]        
        public string 統一編號 { get; set; }
        
        [Required]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string 電話 { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string 傳真 { get; set; }

        [Required]
        [StringLength(100)]
        public string 地址 { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]        
        public string 客戶分類 { get; set; }

        public bool 是否已刪除 { get; set; }
    }

}