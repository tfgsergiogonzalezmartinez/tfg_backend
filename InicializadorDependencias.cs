using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using tfg_backend.interfaces;
using tfg_backend.repositorios;

namespace backend_tfg
{
    public static class InicializadorDependencias
    {
        public static IServiceCollection AddContextMongoDB(
            this IServiceCollection services,                   //ectiendo la clase para añadir nuevos metodos
            IConfiguration configuration, 
            string connectionString,
            string databaseName
        )
        {
            services.AddSingleton(new ContextoDB(connectionString, databaseName));
            return services;
        }  
        public static IServiceCollection AddRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IUserRepositorio,UserRepositorio>();
            services.AddScoped<IChatRepositorio,ChatRepositorio>();
            services.AddScoped<ISoporteRepositorio,SoporteRepositorio>();
            return services;
        }   
    }
}