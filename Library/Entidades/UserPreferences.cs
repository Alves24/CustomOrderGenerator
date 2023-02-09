using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class UserPreferences
    {       
        public bool print { get; set; }
        public string lastSeller { get; set; }
    }
}
