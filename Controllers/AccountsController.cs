using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using AllNewsServer.Configurations;
using AllNewsServer.Data;
using AllNewsServer.Data.Constants;
using AllNewsServer.Data.DTOs.ActiveSession;
using AllNewsServer.Data.DTOs.ApplicationRole;
using AllNewsServer.Data.DTOs.ApplicationUser;
using AllNewsServer.Data.Models;
using AllNewsServer.Services;

namespace AllNewsServer.Controllers {
  // TO DO: Change to refresh token

  [Route ("api/v1/[controller]")]
  public class AccountsController : Controller {
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly ApplicationUserService _applicationUserService;
    private readonly ApplicationRoleService _applicationRoleService;
    public AccountsController (IMapper mapper, ApplicationDbContext context,
      ApplicationUserService applicationUserService, ApplicationRoleService applicationRoleService) {
      _mapper = mapper;
      _context = context;
      _applicationUserService = applicationUserService;
      _applicationRoleService = applicationRoleService;
    }

    [Authorize (Roles = "Admin")]
    [HttpGet ("Roles")]
    [ProducesResponseType (typeof (List<ApplicationRoleDTO>), 200)]
    public async Task<IActionResult> GetAllRolesAsync () {
      var applicationRoles = await _applicationRoleService.GetApplicationRolesAsync ();

      var response = _mapper.Map<List<ApplicationRole>, List<ApplicationRoleDTO>> (applicationRoles);

      return Ok (response);
    }

    [Authorize (Roles = "Admin")]
    [HttpPut ("{id}/Roles")]
    [ProducesResponseType (200)]
    public async Task<IActionResult> UpdateUserRoles (int id, [FromBody] int[] roleIds) {
      var applicationUser = await _applicationUserService.GetApplicationUserAsync (id);

      if (applicationUser != null) {
        foreach (var role in applicationUser.UserRoles.ToList ()) {
          if (!roleIds.Contains (role.ApplicationRoleId))
            applicationUser.UserRoles.Remove (role);
        }

        foreach (var roleId in roleIds) {
          if (!applicationUser.UserRoles.Any (x => x.ApplicationRoleId == roleId)) {
            applicationUser.UserRoles.Add (new UserRole {
              ApplicationRoleId = roleId
            });
          }
        }

        await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

        return NoContent ();
      }

      return NotFound ();
    }

    [Authorize (Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType (typeof (List<ApplicationUserDTO>), 200)]
    public async Task<IActionResult> GetAsync (string query, int offset = 0, int limit = 100) {
      var applicationUsers = await _applicationUserService.GetApplicationUsersAsync (query, offset, limit);

      var response = _mapper.Map<List<ApplicationUser>, List<ApplicationUserDTO>> (applicationUsers);

      return Ok (response);
    }

    [Authorize (Roles = "Admin")]
    [HttpGet ("InRole")]
    [ProducesResponseType (typeof (List<ApplicationUserDTO>), 200)]
    public async Task<IActionResult> GetUsersInRoleAsync (string query, string role, int offset = 0, int limit = 100) {
      var applicationUsers = await _applicationUserService.GetApplicationUsersInRoleAsync (query, role, offset, limit);

      var response = _mapper.Map<List<ApplicationUser>, List<ApplicationUserDTO>> (applicationUsers);

      return Ok (response);
    }

    [Authorize (Roles = "Admin")]
    [HttpGet ("{id}", Name = "GetApplicationUser")]
    [ProducesResponseType (typeof (ApplicationUserDTO), 200)]
    public async Task<IActionResult> GetAsync (int id) {
      var applicationUser = await _applicationUserService.GetApplicationUserAsync (id);

      if (applicationUser != null) {
        var response = _mapper.Map<ApplicationUserDTO> (applicationUser);

        return Ok (response);
      }

      return NotFound ();
    }

    [Authorize (Roles = "Admin")]
    [HttpPost ("{id}/Ban")]
    [ProducesResponseType (typeof (void), 200)]
    public async Task<IActionResult> BanUser (int id) {
      var applicationUser = await _applicationUserService.GetApplicationUserAsync (id);

      if (applicationUser != null) {
        applicationUser.IsBanned = true;

        await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

        return NoContent ();
      }

      return NotFound ();
    }

    [Authorize (Roles = "Admin")]
    [HttpPost ("{id}/Unban")]
    [ProducesResponseType (typeof (void), 200)]
    public async Task<IActionResult> UnbanUser (int id) {
      var applicationUser = await _applicationUserService.GetApplicationUserAsync (id);

      if (applicationUser != null) {
        applicationUser.IsBanned = false;

        await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

        return NoContent ();
      }

      return NotFound ();
    }

    [Authorize (Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> PostAsync (int id, [FromBody] AddApplicationUserDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      if (await _applicationUserService.GetUserByPhoneAsync (model.Phone) != null) {
        ModelState.AddModelError ("User", "Пользователь с таким телефоном уже существует");
        return BadRequest (ModelState);
      }

      var applicationUser = _mapper.Map<ApplicationUser> (model);

      await _applicationUserService.AddApplicationUserAsync (applicationUser);

      return CreatedAtRoute (routeName: "GetApplicationUser",
        routeValues : new { id = applicationUser.Id },
        value : _mapper.Map<ApplicationUserDTO> (applicationUser));
    }

    [HttpPost ("/signup")]
    public async Task<IActionResult> SignupAsync ([FromBody] SignupApplicationUserDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      if (await _applicationUserService.GetUserByPhoneAsync (model.Phone) != null) {
        ModelState.AddModelError ("User", "Пользователь с таким телефоном уже существует");
        return BadRequest (ModelState);
      }

      if (await _applicationUserService.GetUserByEmailAsync (model.Email) != null) {
        ModelState.AddModelError ("User", "Пользователь с таким email уже существует");
        return BadRequest (ModelState);
      }

      var applicationUser = _mapper.Map<ApplicationUser> (model);

      DateTime now = DateTime.UtcNow;

      string ipAddress = "", userAgent = "";

      try {
        ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString ();
        userAgent = Request.Headers["User-Agent"][0];
      } catch {
        Console.WriteLine ("POST /Token: Could not retrieve user information. Creative new active session anyway.");
      }

      var newRefreshToken = GenerateRefreshToken ();

      applicationUser.ActiveSessions = new List<ActiveSession> ();

      applicationUser.ActiveSessions.Add (new ActiveSession {
        RefreshToken = newRefreshToken,
          CreationDateTime = now,
          LastRefreshDateTime = now,
          UserAgent = userAgent,
          IPAddress = ipAddress,
          Location = "",
      });

      applicationUser.UserRoles = new List<UserRole> ();

      // applicationUser.AccessCode = null;
      // applicationUser.AccessCodeExpirationDateTime = null;
      // applicationUser.AccessCodeIssueDateTime = null;
      applicationUser.LoginAttempts = 0;

      await _applicationUserService.AddApplicationUserAsync (applicationUser);

      var identity = GetIdentity (applicationUser);

      var jwt = new JwtSecurityToken (
        issuer: AuthConfiguration.TOKEN_ISSUER,
        audience: AuthConfiguration.TOKEN_AUDIENCE,
        notBefore: now,
        claims: identity.Claims,
        expires: now.Add (TimeSpan.FromMinutes (AuthConfiguration.TOKEN_LIFETIME)),
        signingCredentials: new SigningCredentials (AuthConfiguration.GetSymmetricSecurityKey (), SecurityAlgorithms.HmacSha256));

      var encodedJwt = new JwtSecurityTokenHandler ().WriteToken (jwt);

      var response = new {
        access_token = encodedJwt,
        refresh_token = newRefreshToken,
        expires_in = (AuthConfiguration.TOKEN_LIFETIME) * 60,
        username = identity.Name,
        firstName = applicationUser.FirstName,
        middleName = applicationUser.MiddleName,
        lastName = applicationUser.LastName,
        email = applicationUser.Email,
        roles = applicationUser.UserRoles.Select (x => x.ApplicationRole.Name)
      };

      return Ok (response);

    }

    // Update info of the user specified by id.
    [Authorize (Roles = "Admin")]
    [HttpPut ("{id}")]
    public async Task<IActionResult> PutAsync (int id, [FromBody] UpdateApplicationUserDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      var applicationUser = await _applicationUserService.GetApplicationUserAsync (id);

      if (applicationUser != null) {
        applicationUser = _mapper.Map<UpdateApplicationUserDTO, ApplicationUser> (model, applicationUser);

        await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

        return NoContent ();
      }

      return NotFound ();
    }

    /// Update current user's info.
    [Authorize]
    [HttpPut ("Current")]
    public async Task<IActionResult> PutAsync ([FromBody] UpdateApplicationUserDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      var phone = User.Identity.Name;
      var applicationUser = await _applicationUserService.GetUserByPhoneAsync (phone);

      if (applicationUser != null) {
        applicationUser = _mapper.Map<UpdateApplicationUserDTO, ApplicationUser> (model, applicationUser);

        await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

        return NoContent ();
      }

      return NotFound ();
    }

    [Authorize]
    [HttpGet ("Current")]
    public async Task<IActionResult> GetAsync () {
      var phone = User.Identity.Name;
      var applicationUser = await _applicationUserService.GetUserByPhoneAsync (phone);

      if (applicationUser != null) {
        var response = _mapper.Map<ApplicationUser, ApplicationUserDTO> (applicationUser);

        return Ok (response);
      }

      return NotFound ();
    }

    // [HttpPost("/AccessCode")]
    // public async Task<IActionResult> GetAccessCode()
    // {
    //     // TO DO: Validate phone
    //     string phone = Request.Form["phone"];
    //     string smsToken = Request.Form["smsToken"];

    //     var user = _context.ApplicationUsers.SingleOrDefault(x => x.Phone == phone);

    //     var now = DateTime.UtcNow;

    //     if (user != null)
    //     {
    //         if (user.IsBanned || user.AccessCodeIssueDateTime?.Add(TimeSpan.FromMinutes(AuthConfiguration.ACCESS_CODE_RENEWAL)) > now)
    //         {
    //             return NotFound();
    //         }
    //     }

    //     var accessCode = GenerateAccessCode();

    //     var isSuccessfullySend = await _smsService.SendAccessCodeAsync(phone, accessCode, smsToken);

    //     if (isSuccessfullySend)
    //     {
    //         if (user == null)
    //         {
    //             var executorRole = await _context.ApplicationRoles.SingleOrDefaultAsync(x => x.Name == ApplicationRoleNames.Executor);

    //             user = new ApplicationUser
    //             {
    //                 Phone = phone,
    //                 UserRoles = new List<UserRole>()
    //             };

    //             // The next line is commented out for public but corporate testing.
    //             //
    //             // It has to be uncommented in order to enable users using the app
    //             // without contacting the office.

    //             // user.UserRoles.Add(new UserRole { ApplicationRoleId = executorRole.Id });

    //             await _context.ApplicationUsers.AddAsync(user);
    //         }

    //         user.AccessCodeIssueDateTime = now;
    //         user.AccessCodeExpirationDateTime = now.Add(TimeSpan.FromMinutes(AuthConfiguration.ACCESS_CODE_LIFETIME));
    //         user.AccessCode = accessCode;
    //         user.LoginAttempts = 0;

    //         await _context.SaveChangesAsync();

    //         if (_smsService.IsInMockingMode())
    //             return Ok(accessCode);
    //         else
    //             return NoContent();
    //     }

    //     return BadRequest();
    // }

    [HttpPost ("/Token", Name = "GetToken")]
    public async Task<IActionResult> GetToken ([FromBody] GetTokenApplicationUserDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      var now = DateTime.UtcNow;

      string email = model.Email;
      string password = model.Password;
      string refreshToken = model.RefreshToken;

      ApplicationUser user = null;
      ActiveSession activeSession = null;

      if (!string.IsNullOrEmpty (email) && !string.IsNullOrEmpty (password)) {
        var potentialUser = await _context.ApplicationUsers.SingleOrDefaultAsync (x => x.Email == email);

        if (potentialUser == null) {
          ModelState.AddModelError ("Credentials", "Invalid credentials");
          return BadRequest (ModelState);
        }

        bool isValidPassword = potentialUser.isValidPassword (password);

        if (isValidPassword && (potentialUser.LoginAttempts < 3 || now > potentialUser.LoginBannedUntilDateTime) &&
          !potentialUser.IsBanned) {
          await _context.Entry (potentialUser)
            .Collection (x => x.ActiveSessions)
            .LoadAsync ();

          await _context.Entry (potentialUser)
            .Collection (x => x.UserRoles)
            .Query ()
            .Include (x => x.ApplicationRole)
            .LoadAsync ();

          user = potentialUser;
        } else {
          potentialUser.LoginAttempts++;

          if (potentialUser.LoginAttempts > 3 && potentialUser.LoginBannedUntilDateTime == null) {
            potentialUser.LoginBannedUntilDateTime = DateTime.UtcNow.AddMinutes (5);
          }
          _context.Entry (potentialUser).State = EntityState.Modified;
          _context.SaveChanges ();
        }
      } else if (!string.IsNullOrEmpty (refreshToken)) {
        refreshToken = refreshToken.TrimEnd ('\r', '\n');

        activeSession = await _context.ActiveSessions
          .SingleOrDefaultAsync (x => x.RefreshToken == refreshToken && !x.IsTerminated);

        if (activeSession != null) {
          user = await _context.ApplicationUsers
            .SingleOrDefaultAsync (x => x.Id == activeSession.ApplicationUserId && !x.IsBanned);

          await _context.Entry (user)
            .Collection (x => x.ActiveSessions)
            .LoadAsync ();

          await _context.Entry (user)
            .Collection (x => x.UserRoles)
            .Query ()
            .Include (x => x.ApplicationRole)
            .LoadAsync ();
        }
      }

      if (user != null) {
        var identity = GetIdentity (user);

        var jwt = new JwtSecurityToken (
          issuer: AuthConfiguration.TOKEN_ISSUER,
          audience: AuthConfiguration.TOKEN_AUDIENCE,
          notBefore: now,
          claims: identity.Claims,
          expires: now.Add (TimeSpan.FromMinutes (AuthConfiguration.TOKEN_LIFETIME)),
          signingCredentials: new SigningCredentials (AuthConfiguration.GetSymmetricSecurityKey (), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler ().WriteToken (jwt);

        var newRefreshToken = GenerateRefreshToken ();

        if (activeSession != null) {
          activeSession.RefreshToken = newRefreshToken;
          activeSession.LastRefreshDateTime = now;
        } else {
          user.LoginAttempts = 0;
          user.LoginBannedUntilDateTime = null;

          string ipAddress = "", userAgent = "";

          try {
            ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString ();
            userAgent = Request.Headers["User-Agent"][0];
          } catch {
            Console.WriteLine ("POST /Token: Could not retrieve user information. Creative new active session anyway.");
          }

          user.ActiveSessions.Add (new ActiveSession {
            RefreshToken = newRefreshToken,
              CreationDateTime = now,
              LastRefreshDateTime = now,
              UserAgent = userAgent,
              IPAddress = ipAddress,
              Location = ""
          });
        }

        await _context.SaveChangesAsync ();

        var response = new {
          access_token = encodedJwt,
          refresh_token = newRefreshToken,
          expires_in = (AuthConfiguration.TOKEN_LIFETIME) * 60,
          username = identity.Name,
          firstName = user.FirstName,
          middleName = user.MiddleName,
          lastName = user.LastName,
          email = user.Email,
          roles = user.UserRoles.Select (x => x.ApplicationRole.Name)
        };

        return Ok (response);
      } else {
        ModelState.AddModelError ("Credentials", "Invalid credentials");
        return BadRequest (ModelState);
      }
    }

    [Authorize (Roles = "Admin")]
    [HttpGet ("{id}/ActiveSessions")]
    public async Task<IActionResult> GetUserActiveSessions (int id) {
      var activeSessions = await _applicationUserService.GetUserActiveSessions (id);

      var response = _mapper.Map<List<ActiveSession>, List<ActiveSessionDTO>> (activeSessions);

      return Ok (response);
    }

    [Authorize]
    [HttpGet ("Current/ActiveSessions")]
    public async Task<IActionResult> GetCurrentUserActiveSessions () {
      var phone = User.Identity.Name;
      var applicationUser = await _applicationUserService.GetUserByPhoneAsync (phone);

      if (applicationUser != null) {
        var activeSessions = await _applicationUserService.GetUserActiveSessions (applicationUser.Id);

        var response = _mapper.Map<List<ActiveSession>, List<ActiveSessionDTO>> (activeSessions);

        return Ok (response);
      }

      return NotFound ();
    }

    [Authorize (Roles = "Admin")]
    [HttpPost ("{id}/ActiveSessions/{sessionId}/Terminate")]
    public async Task<IActionResult> TerminateSession (int id, int sessionId) {
      var user = await _applicationUserService.GetApplicationUserWithSessionsAsync (id);

      if (user != null) {
        var activeSession = user.ActiveSessions.SingleOrDefault (x => x.Id == sessionId);

        if (activeSession != null) {
          activeSession.IsTerminated = true;

          await _applicationUserService.UpdateApplicationUserAsync (user);

          return NoContent ();
        }
      }

      return NotFound ();
    }

    [Authorize]
    [HttpPost ("Current/ActiveSessions/{sessionId}/Terminate")]
    public async Task<IActionResult> TerminateSession (int sessionId) {

      var phone = User.Identity.Name;
      var applicationUser = await _applicationUserService.GetUserByPhoneAsync (phone);

      if (applicationUser != null) {
        var user = await _applicationUserService.GetApplicationUserWithSessionsAsync (applicationUser.Id);

        if (user != null) {
          var activeSession = user.ActiveSessions.SingleOrDefault (x => x.Id == sessionId);

          if (activeSession != null) {
            activeSession.IsTerminated = true;

            await _applicationUserService.UpdateApplicationUserAsync (user);

            return NoContent ();
          }
        }
      }

      return NotFound ();
    }

    private ClaimsIdentity GetIdentity (ApplicationUser user) {
      if (user != null) {
        var claims = new List<Claim> {
        new Claim (ClaimsIdentity.DefaultNameClaimType, user.Email),
        // new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin") // user.Role
        };

        foreach (var role in user.UserRoles) {
          claims.Add (new Claim (ClaimTypes.Role, role.ApplicationRole.Name));
        }

        ClaimsIdentity claimsIdentity =
          new ClaimsIdentity (claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
      }

      return null;
    }

    [Authorize]
    [HttpPost ("TerminateCurrentSession")]
    public async Task<IActionResult> TerminateCurrentSession ([FromBody] TerminateSessionDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      var phone = User.Identity.Name;
      var applicationUser = await _applicationUserService.GetUserByPhoneWithSessionsAsync (phone);

      if (applicationUser != null) {
        var activeSession = applicationUser.ActiveSessions.SingleOrDefault (x => x.RefreshToken == model.RefreshToken);

        if (activeSession != null) {
          activeSession.IsTerminated = true;

          await _applicationUserService.UpdateApplicationUserAsync (applicationUser);

          return NoContent ();
        }
      }

      return NotFound ();
    }

    private string GenerateAccessCode () {
      RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider ();
      var byteArray = new byte[4];
      provider.GetBytes (byteArray);

      //convert 4 bytes to an integer
      var randomInteger = BitConverter.ToUInt32 (byteArray, 0);

      var accessCode = randomInteger.ToString ("000000").Substring (0, 6);

      return accessCode;
    }

    private string GenerateRefreshToken () {
      bool exists;
      string refreshToken;

      do {
        refreshToken = GetRandomUniqueToken (400);
        exists = _context.ActiveSessions.Any (x => x.RefreshToken == refreshToken);
      } while (exists);

      return refreshToken;
    }

    private string GetRandomUniqueToken (int maxSize) {
      char[] chars = new char[62];
      chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray ();
      byte[] data = new byte[1];
      using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider ()) {
        crypto.GetNonZeroBytes (data);
        data = new byte[maxSize];
        crypto.GetNonZeroBytes (data);
      }
      StringBuilder result = new StringBuilder (maxSize);
      foreach (byte b in data) {
        result.Append (chars[b % (chars.Length)]);
      }
      return result.ToString ();
    }
  }
}