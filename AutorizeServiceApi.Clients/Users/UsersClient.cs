using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutorizeServiceApi.Clients.Base;
using AutorizeServiceApi.Domain.DTO;
using AutorizeServiceApi.Domain.Models;
using AutorizeServiceApi.Domain.Models.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AutorizeServiceApi.Clients.Users
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration Configuration) : base(Configuration, "api/users") { }

        #region implementation of IDisposable

        void IDisposable.Dispose()
        {
            _Client.Dispose();
        }

        #endregion

        #region Implementation of IUserStore<ApplicationUser>

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/UserId", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/UserName", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(ApplicationUser user, string name, CancellationToken cancel)
        {
            user.UserName = name;
            await PostAsync($"{ServiceAddress}/UserName/{name}", user, cancel);
        }

        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/NormalUserName/", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string name, CancellationToken cancel)
        {
            user.NormalizedUserName = name;
            await PostAsync($"{ServiceAddress}/NormalUserName/{name}", user, cancel);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PutAsync($"{ServiceAddress}/User", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User/Delete", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<ApplicationUser> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<ApplicationUser>($"{ServiceAddress}/User/Find/{id}", cancel);
        }

        public async Task<ApplicationUser> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<ApplicationUser>($"{ServiceAddress}/User/Normal/{name}", cancel);
        }

        #endregion

        #region Implementation of IUserRoleStore<ApplicationUser>

        public async Task AddToRoleAsync(ApplicationUser user, string role, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/Role/{role}", user, cancel);
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string role, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/Role/Delete/{role}", user, cancel);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/roles", user, cancel))
               .Content
               .ReadAsAsync<IList<string>>(cancel);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/InRole/{role}", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return await GetAsync<List<ApplicationUser>>($"{ServiceAddress}/UsersInRole/{role}", cancel);
        }

        #endregion

        #region Implementation of IUserPasswordStore<ApplicationUser>

        public async Task SetPasswordHashAsync(ApplicationUser user, string hash, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetPasswordHash", new PasswordHashDTO { Hash = hash, User = user },
                cancel);
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPasswordHash", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/HasPassword", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserEmailStore<ApplicationUser>

        public async Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancel)
        {
            user.Email = email;
            await PostAsync($"{ServiceAddress}/SetEmail/{email}", user, cancel);
        }

        public async Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetEmail", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetEmailConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancel)
        {
            user.EmailConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/SetEmailConfirmed/{confirmed}", user, cancel);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<ApplicationUser>($"{ServiceAddress}/User/FindByEmail/{email}", cancel);
        }

        public async Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User/GetNormalizedEmail", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }

        public async Task SetNormalizedEmailAsync(ApplicationUser user, string email, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetnormalizedEmail/{email}", user, cancel);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<ApplicationUser>

        public async Task SetPhoneNumberAsync(ApplicationUser user, string phone, CancellationToken cancel)
        {
            user.PhoneNumber = phone;
            await PostAsync($"{ServiceAddress}/SetPhoneNumber/{phone}", user, cancel);
        }

        public async Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPhoneNumber", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPhoneNumberConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancel)
        {
            user.PhoneNumberConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/SetPhoneNumberConfirmed/{confirmed}", user, cancel);
        }

        #endregion

        #region Implementation of IUserLoginStore<ApplicationUser>

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel);
        }

        public async Task RemoveLoginAsync(ApplicationUser user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLogins", user, cancel))
                .Content
                .ReadAsAsync<List<UserLoginInfo>>(cancel);
        }

        public async Task<ApplicationUser> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return await GetAsync<ApplicationUser>($"{ServiceAddress}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel);
        }

        #endregion

        #region Implementation of IUserLockoutStore<ApplicationUser>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLockoutEndDate", user, cancel))
                .Content
                .ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            user.LockoutEnd = EndDate;
            await PostAsync($"{ServiceAddress}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate }, cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/IncrementAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel);
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/ResetAccessFailedCont", user, cancel);
        }

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLockoutEnabled", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetLockoutEnabled/{enabled}", user, cancel);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<ApplicationUser>

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancel)
        {
            user.TwoFactorEnabled = enabled;
            await PostAsync($"{ServiceAddress}/SetTwoFactor/{enabled}", user, cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetTwoFactorEnabled", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserClaimStore<ApplicationUser>

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetClaims", user, cancel))
                .Content
                .ReadAsAsync<List<Claim>>(cancel);
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/AddClaims", new AddClaimDTO { User = user, Claims = claims }, cancel);
        }

        public async Task ReplaceClaimAsync(ApplicationUser user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim }, cancel);
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/RemoveClaims", new RemoveClaimDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetUsersForClaim", claim, cancel))
                .Content
                .ReadAsAsync<List<ApplicationUser>>(cancel);
        }

        #endregion
    }
}
