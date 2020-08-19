using Microsoft.Extensions.Configuration;
using System;
using WebApplication.Utils.Model;

namespace WebApplication.Utils
{
    public class GlobalContext
    {
        public static SystemConfig SystemConfig { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IConfiguration Configuration { get; set; }
    }
}
