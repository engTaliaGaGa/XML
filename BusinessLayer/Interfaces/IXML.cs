using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IXML
    {
        bool CreationXML(string[] fields, List<XMLTemplate> templates);

    }
}
