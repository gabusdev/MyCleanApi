﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    internal static class DatabaseSettingsService
    {
        public static DatabaseSettings GetDbSettings(IConfiguration config)
        {
            // First get Settings from Configuration file
            var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            // The check if there are Settings in Enviroment
            GetSettingsFromEnv(databaseSettings);

            if (databaseSettings.DBProvider == null)
            {
                databaseSettings.DBProvider = "InMemory";
            }
            else if (databaseSettings.DBProvider.ToLower() != "inmemory"
                && databaseSettings.ConnectionString is null)
            {
                throw new InvalidOperationException($"Connection String not provided for {databaseSettings.DBProvider}");
            }
            return databaseSettings;
        }
        private static void GetSettingsFromEnv(DatabaseSettings dBSettings)
        {
            var dbProvider = Environment.GetEnvironmentVariable("dbProvider");
            if (dbProvider != null)
                dBSettings.DBProvider = dbProvider;

            var connString = Environment.GetEnvironmentVariable("connString");
            if (connString != null)
                dBSettings.ConnectionString = connString;
        }
    }
}
