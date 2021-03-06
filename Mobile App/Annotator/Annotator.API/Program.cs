﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Annotator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //Add additional configuration file if necessary
                //.ConfigureAppConfiguration((builderContext, config) =>
                //{
                //    config.AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true);
                //})
                .UseStartup<Startup>()
                .Build();
    }
}
