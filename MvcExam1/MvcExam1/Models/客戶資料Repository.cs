using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Security.Cryptography;

namespace MvcExam1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{

        public 客戶資料 Find(int id)
        {
            var data = this.All().Where(p => p.Id == id).FirstOrDefault();

            return data;
        }

        public 客戶資料 Find(string account)
        {
            var data = this.All().Where(p => p.帳號 == account).FirstOrDefault();

            return data;
        }
        


        public IQueryable<客戶資料> All(bool isBase)
        {
            if (isBase)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }


        public override IQueryable<客戶資料> All()
        {
            var data = base.All().Where(p => p.是否已刪除 == false);
            return data;
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<客戶資料> Query(string keyword, string 客戶分類, string sortColumn, bool desc)
        {
            var data = this.All();

            // 客戶分類過濾
            if (String.IsNullOrEmpty(客戶分類) == false)
            {
                data = data.Where(p => p.客戶分類 == 客戶分類);
            }

            // 關鍵字搜尋
            if (String.IsNullOrEmpty(keyword) == false)
            {

                data = data.Where(p =>
                     p.客戶名稱.Contains(keyword) ||
                     p.統一編號.Contains(keyword) ||
                     p.電話.Contains(keyword) ||
                     p.傳真.Contains(keyword) ||
                     p.地址.Contains(keyword) ||
                     p.Email.Contains(keyword)
                 );
            }

            // 處理排序
            if (!String.IsNullOrEmpty(sortColumn))
            {
                if (desc == false)
                {
                    if (sortColumn == "客戶名稱") data = data.OrderBy(p => p.客戶名稱);
                    if (sortColumn == "帳號") data = data.OrderBy(p => p.帳號);
                    if (sortColumn == "統一編號") data = data.OrderBy(p => p.統一編號);
                    if (sortColumn == "電話") data = data.OrderBy(p => p.電話);
                    if (sortColumn == "傳真") data = data.OrderBy(p => p.傳真);
                    if (sortColumn == "地址") data = data.OrderBy(p => p.地址);
                    if (sortColumn == "Email") data = data.OrderBy(p => p.Email);
                    if (sortColumn == "客戶分類") data = data.OrderBy(p => p.客戶分類);
                }
                else
                {
                    if (sortColumn == "客戶名稱") data = data.OrderByDescending(p => p.客戶名稱);
                    if (sortColumn == "帳號") data = data.OrderByDescending(p => p.帳號);
                    if (sortColumn == "統一編號") data = data.OrderByDescending(p => p.統一編號);
                    if (sortColumn == "電話") data = data.OrderByDescending(p => p.電話);
                    if (sortColumn == "傳真") data = data.OrderByDescending(p => p.傳真);
                    if (sortColumn == "地址") data = data.OrderByDescending(p => p.地址);
                    if (sortColumn == "Email") data = data.OrderByDescending(p => p.Email);
                    if (sortColumn == "客戶分類") data = data.OrderByDescending(p => p.客戶分類);
                }
            }

            return data;
        }

        public byte[] Export(IQueryable<客戶資料> data)
        {            
            // http://no2don.blogspot.com/2013/02/c-nopi-excel-xls.html

            var nameList = new List<string>()
            {
                "Id", "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類"
            };

            var workbook = new HSSFWorkbook();

            // 建立欄位
            var sheet = workbook.CreateSheet("客戶資料");
            for (int i = 0; i < nameList.Count; i++)
            {
                if (i == 0)
                    sheet.CreateRow(0).CreateCell(i).SetCellValue("Id");
                else
                    sheet.GetRow(0).CreateCell(i).SetCellValue(nameList[i]);
            }

            // 建立資料            
            int indexInt = 1;
            foreach(var item in data)
            {
                sheet.CreateRow(indexInt).CreateCell(0).SetCellValue(item.Id);
                sheet.GetRow(indexInt).CreateCell(1).SetCellValue(item.客戶名稱);
                sheet.GetRow(indexInt).CreateCell(2).SetCellValue(item.統一編號);
                sheet.GetRow(indexInt).CreateCell(3).SetCellValue(item.電話);
                sheet.GetRow(indexInt).CreateCell(4).SetCellValue(item.傳真);
                sheet.GetRow(indexInt).CreateCell(5).SetCellValue(item.地址);
                sheet.GetRow(indexInt).CreateCell(6).SetCellValue(item.Email);
                sheet.GetRow(indexInt).CreateCell(7).SetCellValue(item.客戶分類);
                indexInt++;
            }
                       
            // 產生 binary
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            byte[] bs = ms.ToArray();

            return bs;
        }

        public bool CheckLogin(string account, string password)
        {
            var data = this.All().Where(p => p.帳號 == account).FirstOrDefault();
            if (data == null)
                return false;            

            // 比對密碼
            string dbPasswordStr = data.密碼;
            string inputPasswordStr = EncryptPassowrd(password);

            if (dbPasswordStr != inputPasswordStr)
                return false;            

            return true;
        }

        public string EncryptPassowrd(string source)
        {
            string saltStr = "myhash123456";
            byte[] bs = StringToByteArray(saltStr + source);

            MD5 md5 =  MD5.Create();
            byte[] hashBs = md5.ComputeHash(bs);

            string str = Convert.ToBase64String(hashBs);

            return str;
        }

        private byte[] StringToByteArray(string str)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(str);
            return bs;
        }



    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}