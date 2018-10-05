using BotSharp.Core;
using BotSharp.Core.Modules;
using BotSharp.Platform.Abstraction;
using BotSharp.Platform.Articulate.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BotSharp.Platform.Articulate
{
    public class ArticulateModule : IModule
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.TryAddSingleton<IAgentStorageFactory, AgentStorageFactory>();
            services.TryAddSingleton<ArticulateAi<AgentModel>>();

            NLUSetting setting = new NLUSetting();
            config.GetSection("articulateAi").Bind(setting);
            services.AddSingleton(setting);
            services.AddSingleton<AgentStorageInMemory<AgentModel>>();
            services.AddSingleton<AgentStorageInRedis<AgentModel>>();
            services.AddSingleton(factory =>
            {
                Func<string, IAgentStorage<AgentModel>> accesor = key =>
                {
                    if (key.Equals("AgentStorageInRedis"))
                    {
                        return factory.GetService<AgentStorageInRedis<AgentModel>>();
                    }
                    else if (key.Equals("AgentStorageInMemory"))
                    {
                        return factory.GetService<AgentStorageInMemory<AgentModel>>();
                    }
                    else
                    {
                        throw new ArgumentException($"Not Support key : {key}");
                    }
                };
                return accesor;
            });
        }

        public void Configure(IApplicationBuilder app)
        {

        }
    }
}
