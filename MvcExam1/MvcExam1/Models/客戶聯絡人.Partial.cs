using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcExam1.Models
{

    [MetadataType(typeof(客戶聯絡人Metadata))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = new 客戶資料Entities();

            // 如果是新增
            if (this.Id == 0)
            {
                // 同一個客戶 Id 下的資料 Email 不能重覆
                var data = db.客戶聯絡人.Where(p => p.客戶Id == this.客戶Id && p.Email == this.Email).FirstOrDefault();

                if (data != null)
                {
                    yield return new ValidationResult("Email 資料已經存在", new string[] { "Email" });
                }
            }

            // 如果是修改
            if (this.Id != 0)
            {
                // 不是正在修改的這筆, 同一個客戶 Id 下的資料 Email 也不能重覆
                var data = db.客戶聯絡人.Where(p => p.Id != this.Id && p.客戶Id == this.客戶Id && p.Email == this.Email).FirstOrDefault();

                if (data != null)
                {
                    yield return new ValidationResult("Email 資料已經存在", new string[] { "Email" });
                }
            }



        }
    }

    public partial class 客戶聯絡人Metadata
    {
        
        public int Id { get; set; }

        [Required]
        public int 客戶Id { get; set; }

        [Required]
        [StringLength(50)]
        public string 職稱 { get; set; }

        [Required]
        [StringLength(50)]
        public string 姓名 { get; set; }


        [Required]
        [StringLength(250)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [手機驗證格式(ErrorMessage ="手機格式無效, 請輸入, 如: 0911-123456 的內容")]
        public string 手機 { get; set; }

        [Required]
        [StringLength(50)]
        public string 電話 { get; set; }

        public bool 是否已刪除 { get; set; }
    }

}