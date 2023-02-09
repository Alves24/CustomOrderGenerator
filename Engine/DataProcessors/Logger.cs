using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.DataProcessors
{
    public static class Logger
    {
        public static void Error(string errorMesagge)
        {
            var file = new StreamWriter(@"Files\ErrorLog.txt", true);
            file.WriteLine(errorMesagge);
            file.Close();
        }
    }
}
