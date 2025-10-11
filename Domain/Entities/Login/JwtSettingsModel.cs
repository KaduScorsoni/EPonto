using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Login
{
    public class JwtSettingsModel
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = "suaApi";
        public string Audience { get; set; } = "seusClientes";
        public int ExpirationMinutes { get; set; } = 60;
    }
}
