using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PeopleApi.Core.DomainModel.Entities;
using PeopleApi.Core.Mapping;
namespace PeopleApi.Di; 
public static class DiCore {
   public static void AddCore(
      this IServiceCollection services
   ){

      services.AddAutoMapper(typeof(Person), typeof(MappingProfile));
      services.AddAutoMapper(typeof(User), typeof(MappingProfile));
     
      // Auto Mapper Configurations
      new MapperConfiguration(mc => {
         mc.AddProfile(new MappingProfile());
      });
   }
}