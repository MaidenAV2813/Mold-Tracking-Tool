using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPA_DATOS;
using Microsoft.Extensions.DependencyInjection;
using CAPA_NEGOCIO;

namespace CAPA_WEB_API
{
    public static class ContainerExtension
    {

        public static IServiceCollection AddDIContainer(this IServiceCollection services)

        {

            services.AddSingleton<IDataAccess, DataAccess>();

            services.AddTransient<IRoles_Services, Roles_Services>();

            return services;

        }


    }
}
