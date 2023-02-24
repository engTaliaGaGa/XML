using EntityLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IXML
    {
        void CreationXML(string[] fields, List<XMLTemplate> templates);

    }
}
