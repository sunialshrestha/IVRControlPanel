using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVRControlPanel.Models
{
    public class SpreadSheetModel
    {
        public String fileName { get; set; }
        public String[,] contents { get; set; }

    }
}