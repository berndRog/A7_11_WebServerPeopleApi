﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeopleApi.Core.DomainModel.Entities;
using PeopleApi.Core.Misc;
using FluentAssertions;
using Xunit;
namespace BankingApiTest.Persistence.Repositories;
[Collection(nameof(SystemTestCollectionDefinition))]
public  class OwnersRepositoryUt: BaseRepositoryUt {

   private void ShowRepository(string text){
#if DEBUG
      _dataContext.LogChangeTracker(text);
#endif
   }
   
   #region without accounts
   [Fact]
   public async Task FindByIdAsyncUt() {
      // Arrange
      _peopleRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      // Act
      var actual = await _peopleRepository.FindByIdAsync(_seed.Owner1.Id);
      // Assert
      ShowRepository("FindByIdAsync");
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(_seed.Owner1);
   }
   
   [Fact]
   public async Task FindByNameAsyncUt() {
      // Arrange
      _peopleRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      var expected = _seed.Owner2;
      // Act
      var actual = 
         await _peopleRepository.FindByAsync(o => o.Name == "Max Mustermann");   
      // Assert
      ShowRepository("FindByIdNameAsyn");
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(expected);
   }


   [Fact]
   public async Task FindByEmailAsynUt() {
      // Arrange
      _peopleRepository.AddRange(_seed.Owners);
      await _dataContext.SaveAllChangesAsync();
      var expected = _seed.Owner4;
      // Act
      var actual = 
         await _peopleRepository.FindByAsync(o => o.Email.Contains(expected.Email));   
      // Assert
      ShowRepository("FindByEmail");
      actual.Should().NotBeNull()
         .And.BeEquivalentTo(expected);
   }
   
   
   [Fact]
   public async Task AddUt() {
      // Arrange
      Owner owner = new(){
         Id = new Guid("10000000-0000-0000-0000-000000000000"),
         Name = "Erika Mustermann",
         Birthdate = new DateTime(1988, 2, 1).ToUniversalTime(),
         Email = "erika.mustermann@t-online.de"
      }; 
      // Act
      _peopleRepository.Add(owner);
      await _dataContext.SaveAllChangesAsync();
      // Assert
      var actual = await _peopleRepository.FindByIdAsync(owner.Id);
      actual.Should().BeEquivalentTo(owner);
   }
   
   [Fact]
   public async Task AddRangeUt() {
      // Arrange
      var expected = _seed.Owners;
      // Act
      _dataContext.LogChangeTracker("after Add");
      _peopleRepository.AddRange(_seed.Owners);
      _dataContext.LogChangeTracker("after Add");
      await _dataContext.SaveAllChangesAsync();
      _dataContext.LogChangeTracker("after Add");
      // Assert                                  with trac
      // king
      var actual = await _peopleRepository.SelectAsync(true);   
      actual.Should().NotBeNull()
         .And.NotBeEmpty()
         .And.HaveCount(6)
         .And.BeEquivalentTo(expected);
   }
   
   [Fact]
   public async Task UpdateUt() {
      // Arrange
      await _arrangeTest.OwnersAsync(_seed);
      var expected = _seed.Owner1;
      // Act
      var updatedOwner = new Owner {
         Id = expected.Id,
         Name = "Erika Meier",
         Birthdate = expected.Birthdate,
         Email = "erika.meier@icloud.com"
      };
      await _peopleRepository.UpdateAsync(updatedOwner);
      await _dataContext.SaveAllChangesAsync();
      // Assert
      var actual = await _peopleRepository.FindByIdAsync(updatedOwner.Id);
      actual.Should().BeEquivalentTo(updatedOwner);
   }
   #endregion
   
   #region with accounts
   [Fact]
   public async Task FindByIdJoinAsyncUt() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      // Act  with tracking
      var actual = await _peopleRepository.FindByIdJoinAsync(_seed.Owner1.Id, true);
      // Assert
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(_seed.Owner1, options => options.IgnoringCyclicReferences());
   }
   [Fact]
   public async Task FindByJoinAsyncUt() {
      // Arrange
      await _arrangeTest.OwnersWithAccountsAsync(_seed);
      // Act  with tracking
      var actual = await _peopleRepository.FindByJoinAsync(o => o.Email == _seed.Owner5.Email, true);
      // Assert
      actual.Should()
         .NotBeNull().And
         .BeEquivalentTo(_seed.Owner5, options => options.IgnoringCyclicReferences());
   }
   #endregion
}