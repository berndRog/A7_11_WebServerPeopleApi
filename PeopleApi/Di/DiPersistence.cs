using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleApi.Core;
using PeopleApi.Core.DomainModel;
using PeopleApi.Persistence;
using PeopleApi.Persistence.Repositories;
namespace PeopleApi.Di;

public static class DiPersistence {
   public static void AddPersistence(
      this IServiceCollection services,
      IConfiguration configuration,
      bool isTest = false
   ){
      services.AddScoped<IPeopleRepository, PeopleRepository>();
      services.AddScoped<IAuthRepository, AuthRepository>();
      services.AddScoped<ImagesRepository, ImagesRepositoryImpl>();
      
      // Add DbContext (Database) to DI-Container
      var (useDatabase, dataSource) = DataContext.EvalDatabaseConfiguration(configuration);
      
      switch (useDatabase) {
         case "LocalDb":
         case "SqlServer":
            services.AddDbContext<IDataContext, DataContext>(options => 
               options.UseSqlServer(dataSource)
            );
            break;
        case "Sqlite":
            services.AddDbContext<IDataContext, DataContext>(options => 
               options.UseSqlite(dataSource)
            );
            break;
         default:
            throw new Exception("appsettings.json UseDatabase not available");
      }
   }
}