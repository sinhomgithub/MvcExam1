using System;
using System.Linq;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.IO;

namespace MvcExam1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{

        public 客戶銀行資訊 Find(int id)
        {
            var data = this.All().Where(p => p.Id == id).FirstOrDefault();

            return data;
        }


        public IQueryable<客戶銀行資訊> All(bool isBase)
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


        public override IQueryable<客戶銀行資訊> All()
        {
            var data = base.All().Where(p => p.是否已刪除 == false);
            return data;
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<客戶銀行資訊> Query(string keyword)
        {
            var data = this.All();

            if (String.IsNullOrEmpty(keyword) == false)
            {
                data = data.Where(p =>
                    p.帳戶名稱.Contains(keyword) ||
                    p.帳戶號碼.Contains(keyword) ||
                    p.銀行名稱.Contains(keyword)
                );
            }

            data = data.OrderBy(p => p.帳戶名稱);

            return data;
        }


        public byte[] Export(IQueryable<客戶銀行資訊> data)
        {            
            // http://no2don.blogspot.com/2013/02/c-nopi-excel-xls.html
            var nameList = new List<string>()
            {
                "Id", "客戶Id", "銀行名稱", "銀行代碼", "分行代碼", "帳戶名稱", "帳戶號碼"
            };

            var workbook = new HSSFWorkbook();

            // 建立欄位
            var sheet = workbook.CreateSheet("客戶銀行資訊");
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
                sheet.GetRow(indexInt).CreateCell(2).SetCellValue(item.銀行名稱);
                sheet.GetRow(indexInt).CreateCell(3).SetCellValue(item.銀行代碼);
                sheet.GetRow(indexInt).CreateCell(4).SetCellValue(item.分行代碼.HasValue ? item.分行代碼.Value : 0);
                sheet.GetRow(indexInt).CreateCell(5).SetCellValue(item.帳戶名稱);
                sheet.GetRow(indexInt).CreateCell(6).SetCellValue(item.帳戶號碼);                
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

    public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}