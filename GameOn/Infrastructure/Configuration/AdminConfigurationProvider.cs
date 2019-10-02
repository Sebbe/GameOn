using System;
using System.Collections.Generic;
using System.Linq;
using GameOn.Web.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameOn.Web.Infrastructure.Configuration
{
    public class AdminConfigurationProvider : ConfigurationProvider
    {
        private const string Prefix = "gameon:administration:";

        private readonly Action<DbContextOptionsBuilder> _options;

        public AdminConfigurationProvider(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
        }

        public override void Load()
        {
            DbContextOptionsBuilder<GameOnContext> builder = new DbContextOptionsBuilder<GameOnContext>();
            _options(builder);

            using (GameOnContext context = new GameOnContext(builder.Options))
            {
                List<AdminConfiguration> items = context.AdminConfigurations
                                   .AsNoTracking()
                                   .ToList();

                foreach (AdminConfiguration item in items)
                {
                    Data.Add($"{Prefix}{item.Key}", item.Value);
                }
            }
        }

        public override void Set(string key, string value)
        {
            DbContextOptionsBuilder<GameOnContext> builder = new DbContextOptionsBuilder<GameOnContext>();
            _options(builder);

            key = NormalizeKey(key);
            var insertKey = CreateKey(key);

            using (GameOnContext context = new GameOnContext(builder.Options))
            {
                var adminConfiguration = context.AdminConfigurations.FirstOrDefault(x => x.Key == key);
                if (adminConfiguration != default(AdminConfiguration))
                {
                    adminConfiguration.Value = value;
                }
                else
                {
                    context.AdminConfigurations.Add(new AdminConfiguration()
                    {
                        Key   = key,
                        Value = value
                    });
                }

                context.SaveChanges();

                Data[insertKey] = value;
            }
        }

        public override bool TryGet(string key, out string value)
        {
            DbContextOptionsBuilder<GameOnContext> builder = new DbContextOptionsBuilder<GameOnContext>();
            _options(builder);

            key = NormalizeKey(key);

            using (GameOnContext context = new GameOnContext(builder.Options))
            {
                var result = context.AdminConfigurations.AsNoTracking().FirstOrDefault(x => x.Key == key);
                if (!string.IsNullOrEmpty(result?.Value))
                {
                    value = result.Value;
                    return true;
                }
            }

            value = string.Empty;
            return false;
        }

        private string NormalizeKey(string key)
        {
            key = key.ToLower();

            if (key.Contains(":"))
            {
                key = key.Split(":").Last();
            }

            return key;
        }

        private string CreateKey(string key)
        {
            return $"{Prefix}{key}";
        }
    }
}
