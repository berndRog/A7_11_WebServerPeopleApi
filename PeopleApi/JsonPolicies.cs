using System.Text.Json;
namespace PeopleApi;

public class UpperCaseNamingPolicy : JsonNamingPolicy {
   public override string ConvertName(string name) =>
      name.ToLower();
}