using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels
{
    public class ServerResponse
    {
        public DrawingPoint[] DrawingPoints { get; set; }
        public User[] Users { get; set; }
    }
}
