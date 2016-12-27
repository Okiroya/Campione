using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okiroya.Campione.SystemUtility;
using Microsoft.Extensions.Options;
using Okiroya.Campione.SystemUtility.DI;

namespace Okiroya.Campione.Configuration.DI
{
    public static class RegisterDependencyContainerConfigurationConfigureServicesExtensions
    {
        public static void AddRegisterDependencyContainer(this IServiceCollection services, IConfigurationRoot configuration)
        {
            Guard.ArgumentNotNull(services);
            Guard.ArgumentNotNull(configuration);

            services.AddOptions();

            services.Configure<RegisterDependencyContainerOptions>(configuration.GetSection("okiroya.campione/di"));

            new RegisterDependencyContainerFactory(services.BuildServiceProvider().GetRequiredService<IOptions<RegisterDependencyContainerOptions>>());
        }
    }
}
