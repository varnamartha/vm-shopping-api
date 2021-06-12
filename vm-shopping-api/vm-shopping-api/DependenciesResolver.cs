using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vm_shopping_business;
using vm_shopping_business.Business;
using vm_shopping_business.Interfaces;

namespace vm_shopping_api
{
    public class DependenciesResolver
    {
        public static void ServicesResolve(IServiceCollection services)
        {
            services.AddSingleton<IClientBusiness, ClientBusiness>();
            services.AddSingleton<IGatewaySession, GatewaySession>();
            services.AddSingleton<IProductBusiness, ProductBusiness>();
            services.AddSingleton<IOrderBusiness, OrderBusiness>();
        }
    }
}
