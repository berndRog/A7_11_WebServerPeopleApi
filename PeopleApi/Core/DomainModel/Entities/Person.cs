using System;
namespace PeopleApi.Core.DomainModel.Entities;

public class Person : AEntity {
   public override Guid Id { get; init; } = Guid.NewGuid();
   public string FirstName { get; set; } = string.Empty;
   public string LastName { get; set; } = string.Empty;
   public string? Email { get; set; } = null;
   public string? Phone { get; set; } = null;
   public string? localImage { get; set; } = null;
   public string? remoteImage { get; set; } = null;   
}