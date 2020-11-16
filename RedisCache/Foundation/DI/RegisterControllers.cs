using Microsoft.Extensions.DependencyInjection;

using Sitecore.DependencyInjection;

namespace Foundation.DI
{
    class RegisterControllers : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("Feature.*");
            serviceCollection.AddMvcControllers("Foundation.*");
            serviceCollection.AddClassesWithServiceAttribute("Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("Foundation.*");
        }
    }
}
