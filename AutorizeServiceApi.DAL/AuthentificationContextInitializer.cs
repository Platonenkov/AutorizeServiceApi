using System.Diagnostics;
using System.Threading.Tasks;
using AutorizeServiceApi.DAL.Data;
using AutorizeServiceApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutorizeServiceApi.DAL
{
    public class AuthentificationContextInitializer
    {
        private readonly ILogger<AuthentificationContextInitializer> _Logger;
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public AuthentificationContextInitializer(ILogger<AuthentificationContextInitializer> Logger, ApplicationDBContext context
            , UserManager<ApplicationUser> _UserManager, RoleManager<IdentityRole> _RoleManager)
        {
            _Logger = Logger;
            _db = context;
            this._UserManager = _UserManager;
            this._RoleManager = _RoleManager;

            _Logger.LogInformation("Инициализация контекста Базы данных пользователей");
        }

        public async Task InitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Проверка миграций БД Авторизации");
            await _db.Database.MigrateAsync();
            _Logger.LogInformation("Миграции БД Авторизации успешно выполнены. {0:0.###}с", timer.Elapsed.TotalSeconds);
            timer.Stop();

            await InitializeIdentity();
        }
        private async Task InitializeIdentity()
        {
            if (!await _RoleManager.RoleExistsAsync(ApplicationUser.RoleSuperAdmin))
                await _RoleManager.CreateAsync(new IdentityRole(ApplicationUser.RoleSuperAdmin));
            if (!await _RoleManager.RoleExistsAsync(ApplicationUser.RoleAdmin))
                await _RoleManager.CreateAsync(new IdentityRole(ApplicationUser.RoleAdmin));
            if (!await _RoleManager.RoleExistsAsync(ApplicationUser.RoleUser))
                await _RoleManager.CreateAsync(new IdentityRole(ApplicationUser.RoleUser));
            if (await _UserManager.FindByNameAsync(ApplicationUser.SuperAdminUserName) == null)
            {
                var super_admin = new ApplicationUser()
                {
                    UserName = ApplicationUser.SuperAdminUserName,
                    Email = ApplicationUser.SuperAdminEmail
                };
                var creation_result = await _UserManager.CreateAsync(super_admin, ApplicationUser.SuperAdminDefaultPassword);
                if (creation_result.Succeeded)
                    await _UserManager.AddToRoleAsync(super_admin, ApplicationUser.RoleSuperAdmin);
            }
        }
    }
}
