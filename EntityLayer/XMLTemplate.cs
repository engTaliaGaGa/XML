using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer
{
    public class XMLTemplate
    {
        public int IdTemplate { get; set; }

        public int IdMapClient { get; set; } 

        public string Section { get; set; }

        public string Row { get; set; }
        
        public string Column { get; set; }
        
        public string ParentElement { get; set; }
        
        public string Element { get; set; }
        
        public string Attribute { get; set; }
        
        public string FillWith { get; set; }
        
        public int? IdType { get; set; }
        
        public bool IsRequeried { get; set; }
    }
}
