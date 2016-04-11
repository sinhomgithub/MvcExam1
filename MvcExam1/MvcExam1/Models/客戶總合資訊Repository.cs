using System;
using System.Linq;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.IO;

namespace MvcExam1.Models
{   
	public  class 客戶總合資訊Repository : EFRepository<客戶總合資訊>, I客戶總合資訊Repository
	{
        public 客戶總合資訊 Find(int id)
        {
            var data = this.All().Where(p => p.Id == id).FirstOrDefault();

            return data;
        }


        public IQueryable<客戶總合資訊> Query(string keyword)
        {
            var data = this.All();

            if (String.IsNullOrWhiteSpace(keyword) == false)
            {
                data = data.Where(p => p.客戶名稱.Contains(keyword));
            }

            data = data.OrderBy(p => p.客戶名稱);

            return data;
        }

        public byte[] Export(IQueryable<客戶總合資訊> data)
        {
                        
            // http://no2don.blogspot.com/2013/02/c-nopi-excel-xls.html
            var nameList = new List<string>()
            {
                "Id", "客戶名稱", "聯絡人數量", "銀行帳戶數量"
            };

            var workbook = new HSSFWorkbook();

            // 建立欄位
            var sheet = workbook.CreateSheet("客戶總合資訊");
            for (int i = 0; i < nameList.Count; i++)
            {
                if (i == 0)
                    sheet.CreateRow(0).CreateCell(i).SetCellValue(nameList[i]);
                else
                    sheet.GetRow(0).CreateCell(i).SetCellValue(nameList[i]);
            }

            // 建立資料            
            int indexInt = 1;
            foreach (var item in data)
            {
                sheet.CreateRow(indexInt).CreateCell(0).SetCellValue(item.Id);
                sheet.GetRow(indexInt).CreateCell(1).SetCellValue(item.客戶名稱);
                sheet.GetRow(indexInt).CreateCell(2).SetCellValue(item.聯絡人數量.HasValue ? item.聯絡人數量.Value: 0 );
                sheet.GetRow(indexInt).CreateCell(3).SetCellValue(item.銀行帳戶數量.HasValue ? item.銀行帳戶數量.Value : 0);
                indexInt++;
            }

            // 產生 binary
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            byte[] bs = ms.ToArray();

            return bs;
        }


    }

	public  interface I客戶總合資訊Repository : IRepository<客戶總合資訊>
	{

	}
}