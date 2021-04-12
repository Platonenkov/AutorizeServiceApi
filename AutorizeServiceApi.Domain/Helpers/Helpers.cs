using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AutorizeServiceApi.Domain.Helpers
{
    public static class Helpers
    {
        public static string GetConfiguration(this IConfiguration configuration, string section, string name)
        {
            return configuration?.GetSection(section)?[name];
        }
    }
}
