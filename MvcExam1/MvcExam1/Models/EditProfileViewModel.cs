using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcExam1.Models
{
    public class EditProfileViewModel
    {

        
        public string 帳號 { get; set; }
        public string 統一編號 { get; set; }
        public string 客戶分類 { get; set; }
        public string 客戶名稱 { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [MaxLength(250, ErrorMessage = "密碼最多250個字")]        
        [DataType(DataType.Password)]
        public string 密碼 { get; set; }

        [DisplayName("確認密碼")]
        [DataType(DataType.Password)]
        [Compare("密碼", ErrorMessage = "密碼與確認密碼不符")]
        public string 確認密碼 { get; set; }

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


       


    }
}