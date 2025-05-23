using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Models
{
    public class PlcDataMessage
    {
        public int IsConnectPLC { get; set; }
        public string IsPosition { get; set; }
        public string IsSpeed { get; set; }
        public string CurrentPos { get; set; }
    }
}
