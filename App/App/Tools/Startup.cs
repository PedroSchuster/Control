using System;
using System.Collections.Generic;
using System.Text;
using App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Init()
        {
            var serviceProvider = new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider();

            ServiceProvider = serviceProvider;

            return serviceProvider;
        }

    }
}
