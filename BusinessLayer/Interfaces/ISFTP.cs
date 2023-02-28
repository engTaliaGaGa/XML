using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ISFTP
    {
        List<string> ConnectionSFTP();

        void ErrorFile(string filename);
    }
}
