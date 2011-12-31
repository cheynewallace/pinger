using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pinger
{
    class FileSysSetup
    {
        public static void SetupFileSys()
        {
            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }
        }

        public static bool CheckForHosts()
        {
            if(File.Exists("hosts.txt"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
