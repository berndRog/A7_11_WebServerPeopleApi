using AutoMapper;
using PeopleApi.Core.DomainModel.Entities;
using PeopleApi.Core.Dto;
namespace PeopleApi.Core.Mapping;

 public class MappingProfile : Profile {
   
   public MappingProfile() {
      CreateMap<Person, PersonDto>()
         .ReverseMap();
      CreateMap<User, UserDto>()
         .ReverseMap();
   }
}
