using System.ComponentModel.DataAnnotations;

namespace MvcExam1.Models
{
    public class EditProfileViewModel
    {

        public int Id { get; set; }


        public string 密碼;

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