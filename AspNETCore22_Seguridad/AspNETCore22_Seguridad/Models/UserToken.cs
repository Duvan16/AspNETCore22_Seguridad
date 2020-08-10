using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNETCore22_Seguridad.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
