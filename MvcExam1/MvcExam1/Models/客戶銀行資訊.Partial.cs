using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcExam1.Models
{

    [MetadataType(typeof(客戶銀行資訊Metadata))]
    public partial class 客戶銀行資訊
    {        
    }

    public class 客戶銀行資訊Metadata
    {        
        
        public int Id { get; set; }

        [Required]        
        public int 客戶Id { get; set; }

        [Required]
        [StringLength(50)]
        public string 銀行名稱 { get; set; }

        [Required]        
        public int 銀行代碼 { get; set; }

        public Nullable<int> 分行代碼 { get; set; }

        [Required]
        [StringLength(50)]
        public string 帳戶名稱 { get; set; }

        [Required]
        [StringLength(20)]
        public string 帳戶號碼 { get; set; }


        public bool 是否已刪除 { get; set; }
    }

}