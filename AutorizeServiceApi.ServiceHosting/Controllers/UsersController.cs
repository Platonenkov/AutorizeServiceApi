using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutorizeServiceApi.DAL.Data;
using AutorizeServiceApi.Domain.DTO;
using AutorizeServiceApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutorizeServiceApi.ServiceHosting.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly UserStore<ApplicationUser> _UserStore;

        public UsersController(ApplicationDBContext db) => _UserStore = new UserStore<ApplicationUser>(db);

        [HttpGet("AllUsers")]
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers() => await  _UserStore.Users.ToArrayAsync();

        [HttpPost("UserId")]
        public async Task<string> GetUserIdAsync([FromBody] ApplicationUser user) => await _UserStore.GetUserIdAsync(user);

        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] ApplicationUser user) => await _UserStore.GetUserNameAsync(user);

        [HttpPost("UserName/{name}")]
        public async Task SetUserNameAsync([FromBody] ApplicationUser user, string name) => await _UserStore.SetUserNameAsync(user, name);

        [HttpPost("NormalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] ApplicationUser user) => await _UserStore.GetNormalizedUserNameAsync(user);

        [HttpPost("NormalUserName/{name}")]
        public Task SetNormalizedUserNameAsync([FromBody] ApplicationUser user, string name) => _UserStore.SetNormalizedUserNameAsync(user, name);

        [HttpPost("User")]
        public async Task<bool> CreateAsync([FromBody] ApplicationUser user) => (await _UserStore.CreateAsync(user)).Succeeded;

        [HttpPut("User")]
        public async Task<bool> UpdateAsync([FromBody] ApplicationUser user) => (await _UserStore.UpdateAsync(user)).Succeeded;

        [HttpPost("User/Delete")]
        public async Task<bool> DeleteAsync([FromBody] ApplicationUser user) => (await _UserStore.DeleteAsync(user)).Succeeded;

        [HttpGet("User/Find/{id}")]
        public async Task<ApplicationUser> FindByIdAsync(string id) => await _UserStore.FindByIdAsync(id);

        [HttpGet("User/Normal/{name}")]
        public async Task<ApplicationUser> FindByNameAsync(string name) => await _UserStore.FindByNameAsync(name);

        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] ApplicationUser user, string role) => await _UserStore.AddToRoleAsync(user, role);

        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] ApplicationUser user, string role) => await _UserStore.RemoveFromRoleAsync(user, role);

        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] ApplicationUser user) => await _UserStore.GetRolesAsync(user);

        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] ApplicationUser user, string role) => await _UserStore.IsInRoleAsync(user, role);

        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role) => await _UserStore.GetUsersInRoleAsync(role);

        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] ApplicationUser user) => await _UserStore.GetPasswordHashAsync(user);

        [HttpPost("SetPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _UserStore.SetPasswordHashAsync(hash.User, hash.Hash);
            return hash.User.PasswordHash;
        }

        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] ApplicationUser user) => await _UserStore.HasPasswordAsync(user);

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] ApplicationUser user) => await _UserStore.GetClaimsAsync(user);

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO ClaimInfo) => await _UserStore.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo) =>
            await _UserStore.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.Claim, ClaimInfo.NewClaim);

        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO ClaimInfo) =>
            await _UserStore.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync([FromBody] Claim claim) => await _UserStore.GetUsersForClaimAsync(claim);

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] ApplicationUser user) => await _UserStore.GetTwoFactorEnabledAsync(user);

        [HttpPost("SetTwoFactor/{enable}")]
        public async Task SetTwoFactorEnabledAsync([FromBody] ApplicationUser user, bool enable) => await _UserStore.SetTwoFactorEnabledAsync(user, enable);

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] ApplicationUser user) => await _UserStore.GetEmailAsync(user);

        [HttpPost("SetEmail/{email}")]
        public async Task SetEmailAsync([FromBody] ApplicationUser user, string email) => await _UserStore.SetEmailAsync(user, email);

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] ApplicationUser user) => await _UserStore.GetEmailConfirmedAsync(user);

        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task SetEmailConfirmedAsync([FromBody] ApplicationUser user, bool enable) => await _UserStore.SetEmailConfirmedAsync(user, enable);

        [HttpGet("UserFindByEmail/{email}")]
        public async Task<ApplicationUser> FindByEmailAsync(string email) => await _UserStore.FindByEmailAsync(email);

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] ApplicationUser user) => await _UserStore.GetNormalizedEmailAsync(user);

        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task SetNormalizedEmailAsync([FromBody] ApplicationUser user, string email) =>
            await _UserStore.SetNormalizedEmailAsync(user, email);

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] ApplicationUser user) => await _UserStore.GetPhoneNumberAsync(user);

        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task SetPhoneNumberAsync([FromBody] ApplicationUser user, string phone) => await _UserStore.SetPhoneNumberAsync(user, phone);

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] ApplicationUser user) => await _UserStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task SetPhoneNumberConfirmedAsync([FromBody] ApplicationUser user, bool confirmed) =>
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO login) => await _UserStore.AddLoginAsync(login.User, login.UserLoginInfo);

        [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]
        public async Task RemoveLoginAsync([FromBody] ApplicationUser user, string LoginProvider, string ProviderKey) =>
            await _UserStore.RemoveLoginAsync(user, LoginProvider, ProviderKey);

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] ApplicationUser user) => await _UserStore.GetLoginsAsync(user);

        [HttpGet("User/FindByLogin/{LoginProvider}/{ProviderKey}")]
        public async Task<ApplicationUser> FindByLoginAsync(string LoginProvider, string ProviderKey) =>
            await _UserStore.FindByLoginAsync(LoginProvider, ProviderKey);

        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] ApplicationUser user) => await _UserStore.GetLockoutEndDateAsync(user);

        [HttpPost("SetLockoutEndDate")]
        public async Task SetLockoutEndDateAsync([FromBody] SetLockoutDTO LocoutInfo) =>
            await _UserStore.SetLockoutEndDateAsync(LocoutInfo.User, LocoutInfo.LockoutEnd);

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] ApplicationUser user) => await _UserStore.IncrementAccessFailedCountAsync(user);

        [HttpPost("ResetAccessFailedCount")]
        public async Task ResetAccessFailedCountAsync([FromBody] ApplicationUser user) => await _UserStore.ResetAccessFailedCountAsync(user);

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] ApplicationUser user) => await _UserStore.GetAccessFailedCountAsync(user);

        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] ApplicationUser user) => await _UserStore.GetLockoutEnabledAsync(user);

        [HttpPost("SetLockoutEnabled/{enable}")]
        public async Task SetLockoutEnabledAsync([FromBody] ApplicationUser user, bool enable) => await _UserStore.SetLockoutEnabledAsync(user, enable);
    }
}