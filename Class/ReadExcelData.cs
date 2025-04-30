using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Class
{
    public class ReadExcelData
    {
        string basePath;
        string pathExcel;
        public ReadExcelData()
        {
            basePath = AppContext.BaseDirectory;
            pathExcel = Path.Combine(basePath, "Employees2.xls");
        }
    }
}
