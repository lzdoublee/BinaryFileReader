using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileReader
{
    public class TeamDetails
    {
        public string FileID { get; set; }
        public string TeamName { get; set; }
        public string Manager { get; set; }
        public Int32 Players { get; set; }
    }
}