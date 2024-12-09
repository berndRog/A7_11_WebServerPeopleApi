using AutoMapper;
using PeopleApi.Controllers;
using PeopleApi.Controllers.V2;
using PeopleApi.Core.DomainModel.Entities;
using PeopleApi.Core.Mapping;
using Microsoft.Extensions.DependencyInjection;
namespace BankingApiTest.Di.V2;

public static class DiControllersTest {
   public static IServiceCollection AddControllersTest(
      this IServiceCollection services
   ) {
      services.AddAutoMapper(typeof(Owner), typeof(MappingProfile));
      // Auto Mapper Configurations
      var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
      
      // Controllers
      services.AddScoped<OwnersController>();
      services.AddScoped<AccountsController>();
      services.AddScoped<BeneficiariesController>();
      services.AddScoped<TransfersController>();
      services.AddScoped<TransactionsController>();

      return services;
   }
}