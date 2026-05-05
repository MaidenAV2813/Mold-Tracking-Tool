using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPA_DATOS;
using CAPA_NEGOCIO;

namespace Tracking_Tool_System.App_Start
{
    public static class ContainerExtensions
    {

        public static IServiceCollection AddDIContainer(this IServiceCollection services)

        {

            services.AddSingleton<IDataAccess, DataAccess>();

            services.AddTransient<IRoles_Services, Roles_Services>();

            return services;

        }


    }
}
