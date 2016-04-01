using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MvcExam1.Models
{   
	public  class 客戶分類清單Repository : EFRepository<客戶分類清單>, I客戶分類清單Repository
	{

	}

	public  interface I客戶分類清單Repository : IRepository<客戶分類清單>
	{

	}
}