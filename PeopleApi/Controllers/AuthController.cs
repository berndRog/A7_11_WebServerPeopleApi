// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using Asp.Versioning;
// using AutoMapper;
// using PeopleApi.Core;
// using PeopleApi.Core.DomainModel;
// using PeopleApi.Core.DomainModel.Entities;
// using PeopleApi.Core.Dto;
// using PeopleApi.Core.Password;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
//
// namespace BankingClient.Controllers;
//
// [ApiVersion("1.0")]
// [Route("peopleapi/v{version:apiVersion}")]
//
// [ApiController]
// public class AuthController(
//    IAuthRepository authRepository,
//    IDataContext dataContext,
//    IMapper mapper,
//    IConfiguration configuration
// ) : ControllerBase {
//    
//    /// <summary>
//    /// Login a user
//    /// </summary>
//    /// <param name="userDto"></param>
//    /// <returns>access token</returns>
//    [HttpPost("auth/login")]
//    public async Task<IActionResult> Login(
//       [FromBody] UserDto userDto
//    ) {
//       // check if user exists
//       var foundUser = await authRepository.FindByAsync(u => u.Username == userDto.Username);
//       if(foundUser == null) {
//          return NotFound("Username not found");
//       }
//       
//       // verify password
//       if (!PasswordHasher.Verify(userDto.Password, foundUser.Hashed, foundUser.Salted)) 
//          return Unauthorized();
//       
//       // return the authorization code
//       var authCode = configuration["JwtSettings:AuthorizationCode"];
//       if(authCode == null) {
//          return NotFound("Authorization not granted");
//       }
//       
//       return Ok( new { authCode = authCode } );
//
//    }
//    
//    /// <summary>
//    /// Get Access Token
//    /// </summary>
//    /// <param name="userDto"></param>
//    /// <returns>Bearer JWT with Id Token, Access Token and refereh token</returns>
//    [HttpPost("auth/token")]
//    public async Task<IActionResult> Authorize(
//       [FromForm] FormFile formfile
//    ) {
//       
//       
//       // var grantType = formfile.GetGrantType();
//       // var clientId = formfile.GetUserId();
//       // var clientSecret = formfile.GetClientSecret();
//       // var audience = formfile.GetAudience();
//       
//       // check if user exists
//       var foundUser = await authRepository.FindByAsync(u => u.Id == Guid.NewGuid());
//       if(foundUser == null) {
//          return NotFound("Username not found");
//       }
//       
//  
//       // // Create a Bearer token
//       // //var token = "login granted"; //GenerateJwtToken(userDto.Username, foundUser.Salted);
//       // var token = GenerateIdToken(foundUser.Id, userDto.Username);
//       //
//       return Ok( new { Token = "" } );
//    }
//    
//    private string GenerateIdToken(Guid userId, string username) {
//       
//       var secret = configuration["JwtSettings:SecretKey"];
//       var secretKey   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
//       var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
//       
//       // Oidc/OAuth-Server -> PeopleApi server
//       var iss = configuration["JwtSettings:Issuer"];
//       // Ressource-Server  -> PeopleApi server
//       var aud = configuration["JwtSettings:Audience"];
//
//       var claims = new List<Claim> {
//          // Unique indentifier of the user
//          new Claim(ClaimTypes.Name, username),
//          new Claim(ClaimTypes.Role, "User"),
//       };
//       
//       var tokeOptions = new JwtSecurityToken(
//          issuer: iss, 
//          audience: aud, 
//          claims: new List<Claim> {
//             new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
//             new Claim(ClaimTypes.Name, username)            
//          }, 
//          expires: DateTime.UtcNow.AddHours(6),  // exp as epoch
//          notBefore: DateTime.UtcNow,            // 
//          signingCredentials: credentials
//       );
//       
//       var tokenHandler = new JwtSecurityTokenHandler();
//       var token = tokenHandler.WriteToken(tokeOptions);
//       
//       return token;
//       
//       // var tokenDescriptor = new SecurityTokenDescriptor {
//       //    Subject = new ClaimsIdentity(new[] {
//       //       new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
//       //    }),
//       //    Expires = DateTime.UtcNow.AddHours(2),
//       //    SigningCredentials = credentials
//       // };
//       // var token = tokenHandler.CreateToken(tokenDescriptor);
//       // return tokenHandler.WriteToken(token);
//    }
//
//    
//    // Generating token based on user information
//    private JwtSecurityToken GenerateAccessToken(string userId) {
//       
//       var secret = configuration["JwtSettings:SecretKey"];
//       var secretKey   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
//       var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
//       
//       // Oidc/OAuth-Server -> PeopleApi server
//       var iss = configuration["JwtSettings:ValidIssuer"];
//       // Ressource-Server  -> PeopleApi server
//       var aud = configuration["JwtSettings:ValidAudience"];
//       
//       // scope       Specifies the permissions or scopes granted by the token.
//       //             Example "scope": "read write"
//       // roles       Specifies the roles assigned to the user. 
//       //             Example: "roles": ["admin", "user"]
//       // username    Provides the username of the authenticated user
//       // email       provides the emaul of the authenticated user
//       //             Example: "email": "user@example.com"
//       // permissions A list of specific permissions granted to the user.
//       //             Example: "permissions": ["view_dashboard", "edit_profile"]
//       var claims = new List<Claim> {
//          // Unique indentifier of the user
//          new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
//          new Claim(ClaimTypes.Role, "User"),
//       };
//
//       // Create a JWT
//       var token = new JwtSecurityToken(
//          issuer: iss,
//          audience: aud,
//          claims: claims,
//          expires: DateTime.UtcNow.AddHours(24), // Token expiration time
//          signingCredentials: credentials
//       );
//
//       return token;
//    }
//    
//    
//    [HttpPost("logout")]
//    public IActionResult Logout() {
//       // Simulate logout
//       return Ok();
//    }
//
//    [HttpPost("register")]
//    public async Task<ActionResult<UserDto>> Register(
//       [FromBody] UserDto userDto
//    ) {
//       
//       // Check if username already exists
//       if(await authRepository.FindByAsync(u => u.Username == userDto.Username) != null) {
//          return BadRequest(new { message = "Username already exists" });
//       }
//       
//       // Hash and salt the password
//       var(hashed, salted) = PasswordHasher.Hash(userDto.Password);
//       var user = new User {
//          Id = userDto.Id,
//          Username = userDto.Username,
//          Hashed = hashed,
//          Salted = salted,
//          PersonId = userDto.PersonId
//       };
//       
//       // Save the user
//       authRepository.Add(user);
//       await dataContext.SaveAllChangesAsync();
//       
//       var path = Request == null 
//          ? $"/banking/v2/register/{user.Id}" 
//          : $"{Request.Path}/{user.Id}";
//       
//       var uri = new Uri(path, UriKind.Relative);
//       userDto = new UserDto(user.Id, user.Username, "", user.PersonId);
//       return Created(uri: uri, value: userDto);
//    }
// }