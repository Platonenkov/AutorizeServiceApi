using Microsoft.AspNetCore.Identity;

namespace AutorizeServiceApi.Domain.Models.Base
{
    public interface IUsersClient : IUserRoleStore<ApplicationUser>,
                                    IUserClaimStore<ApplicationUser>,
                                    IUserPasswordStore<ApplicationUser>,
                                    IUserTwoFactorStore<ApplicationUser>,
                                    IUserEmailStore<ApplicationUser>,
                                    IUserPhoneNumberStore<ApplicationUser>,
                                    IUserLoginStore<ApplicationUser>,
                                    IUserLockoutStore<ApplicationUser>
    {
    }
}
