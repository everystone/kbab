using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma3Favorites
{
   public class favorite
    {
        public String host;
        public int port;

        public favorite(String host, int ip)
        {
            this.host = host;
            this.port = ip;
        }
    }
}
