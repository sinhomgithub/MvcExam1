using System;
using System.Linq;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.IO;

namespace MvcExam1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            var data = this.All().Where(p => p.Id == id).FirstOrDefault();

            return data;
        }


        public IQueryable<客戶聯絡人> All(bool isBase)
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


        public override IQueryable<客戶聯絡人> All()
        {
            var data = base.All().Where(p => p.是否已刪除 == false);
            return data;
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<客戶聯絡人> QueryBy客戶Id(int 客戶Id)
        {
            var data = this.All();
            data = data.Where(p => p.客戶Id == 客戶Id);
            return data;
        }


        public IQueryable<客戶聯絡人> Query(string keyword)
        {
            var data = this.All();

            if (String.IsNullOrEmpty(keyword) == false)
            {
                data = data.Where(p =>
                   p.Email.Contains(keyword)  ||
                   p.姓名.Contains(keyword)  ||
                   p.手機.Contains(keyword)  ||
                   p.職稱.Contains(keyword)  ||
                   p.電話.Contains(keyword)                   
               );
            }

            return data;
        }


        public byte[] Export(IQueryable<客戶聯絡人> data)
        {        
            // http://no2don.blogspot.com/2013/02/c-nopi-excel-xls.html
            var nameList = new List<string>()
            {
                "Id", "客戶Id", "職稱", "姓名", "Email", "手機", "電話"
            };

            var workbook = new HSSFWorkbook();

            // 建立欄位
            var sheet = workbook.CreateSheet("客戶聯絡人");
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
                sheet.GetRow(indexInt).CreateCell(1).SetCellValue(item.客戶Id);
                sheet.GetRow(indexInt).CreateCell(2).SetCellValue(item.職稱);
                sheet.GetRow(indexInt).CreateCell(3).SetCellValue(item.姓名);
                sheet.GetRow(indexInt).CreateCell(4).SetCellValue(item.Email);
                sheet.GetRow(indexInt).CreateCell(5).SetCellValue(item.手機);
                sheet.GetRow(indexInt).CreateCell(6).SetCellValue(item.電話);
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

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}