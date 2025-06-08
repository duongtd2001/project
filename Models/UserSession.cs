using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Models
{
    public static class UserSession
    {
        public static string CurrentUser { get; set; }
        public static string CurrentID { get; set; }
        public static string CurrentAccess { get; set; }
        public static string CurrentPO { get; set; }
        public static int NumberOfLoginTimes { get; set; }
        public static string SavePos { get; set; }
        public static int CurrentPos { get; set; }
    }
}
