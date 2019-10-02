using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameOn.Web.Infrastructure.Configuration
{
    public static class AdminConfigurationExtensions
    {
        public static IConfigurationBuilder AddAdminConfigurationBuilder(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup)
        {
            configuration.Add(new AdminConfigurationSource(setup));
            return configuration;
        }
    }
}
