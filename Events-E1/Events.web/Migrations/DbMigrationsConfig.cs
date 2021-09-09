namespace Events.web.Migrations
{
    using Events.web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<Events.web.Models.ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(Events.web.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminFullName = "Administrator";
                var adminUserName = adminEmail;
                var adminPassword = adminEmail;
                var adminDateTime = DateTime.Now;
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole, adminDateTime);
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = new IdentityRole();
                roleManager.Create(new IdentityRole("Trainer"));
                roleManager.Create(new IdentityRole("TrainingStaff"));
                roleManager.Create(new IdentityRole("Trainee"));
            }
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole, DateTime adminDateTime)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                DateofBirth = adminDateTime,
                Email = adminEmail,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }
    }
}
