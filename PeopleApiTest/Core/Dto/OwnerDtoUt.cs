using AutoMapper;
using PeopleApi.Core.Dto;
using PeopleApi.Core.Mapping;
using FluentAssertions;
using Xunit;
namespace BankingApiTest.Core.Dto;

public class OwnerDtoUt {
   private readonly Seed _seed;

   public OwnerDtoUt(){
      var config = new MapperConfiguration(config =>
         config.AddProfile(new MappingProfile())
      );
      var mapper = new Mapper(config);
      _seed = new Seed();
   }
   
   [Fact]
   public void CtorAndGetterUt(){
      // Arrange
      // Act
      var actual = new OwnerDto(
         Id: _seed.Owner1.Id,
         Name: _seed.Owner1.Name,
         Birthdate: _seed.Owner1.Birthdate,
         Email: _seed.Owner1.Email
      );
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<OwnerDto>();
      actual.Id.Should().Be(_seed.Owner1.Id);
      actual.Name.Should().Be(_seed.Owner1.Name);
      actual.Birthdate.Should().Be(_seed.Owner1.Birthdate);
      actual.Email.Should().Be(_seed.Owner1.Email);
   }
}