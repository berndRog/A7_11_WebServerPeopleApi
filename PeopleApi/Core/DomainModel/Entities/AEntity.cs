using System;
namespace PeopleApi.Core.DomainModel.Entities;
public abstract class AEntity {
   #region properties
   public abstract Guid Id { get; init; }
   #endregion
}