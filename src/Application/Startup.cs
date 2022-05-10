﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication (this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services
                .AddValidatorsFromAssembly(assembly)
                .AddMediatR(assembly);
            
            return services;
        }
    }
}