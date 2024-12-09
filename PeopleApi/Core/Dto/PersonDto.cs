using System;
namespace PeopleApi.Core.Dto;

/// <summary>
/// PersonDto 
/// </summary>
public record PersonDto(
   Guid Id,
   string FirstName,
   string LastName,
   string? Email,
   string? Phone,
   string? LocalImage,
   string? RemoteImage
);