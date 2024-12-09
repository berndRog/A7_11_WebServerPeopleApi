using System;
namespace PeopleApi.Core.Dto;

public record UserDto(
   Guid Id,
   string Username,
   string Password,
   Guid PersonId
);
