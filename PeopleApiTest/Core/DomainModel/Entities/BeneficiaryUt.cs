using System;
using FluentAssertions;
using PeopleApi.Core.DomainModel.Entities;
using Xunit;

namespace BankingApiTest.Core.DomainModel.Entities;
public class BeneficiaryUt {

   private readonly Seed _seed;
   
   public BeneficiaryUt() {
      _seed = new Seed();
   }

   #region without 
   [Fact]
   public void Ctor() {
      // Arrange
      // Act
      var actual = new Beneficiary();
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Beneficiary>();
   }
   [Fact]
   public void ObjectInitializerUt() {
      // Arrange
      var id = _seed.Beneficiary1.Id;
      var name = _seed.Beneficiary1.Name;
      var iban = _seed.Beneficiary1.Iban;
      // Act
      var actual = new Beneficiary {
         Id = id,
         Name = name,
         Iban = iban,
         AccountId = _seed.Account1.Id
      };
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Beneficiary>();
      actual.Id.Should().Be(id);
      actual.Name.Should().Be(name);
      actual.Iban.Should().Be(iban);
      actual.AccountId.Should().Be(_seed.Account1.Id);
   }
   [Fact]
   public void SetterGetterUt() {
      // Arrange
      var id = new Guid("a1000000-0000-0000-0000-000000000000");
      const string name = "Fritz Fischer";
      const string iban = "DE99 1000 0000 0000 0000";
      // Act
      var actual = new Beneficiary{
         Id = id,
         Iban = iban,
         Name = name
      };
      actual.AccountId = _seed.Account1.Id;
      var _id = actual.Id;
      var _name = actual.Name;
      var _iban = actual.Iban;
      var _accountId = actual.AccountId;
      // Assert
      _id.Should().Be(id);
      _name.Should().Be(name);
      _iban.Should().Be(iban);
      _accountId.Should().Be(_seed.Account1.Id);
   }
   #endregion
}