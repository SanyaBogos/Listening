using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace WebListening.Models
{
    public static class SampleData
    {
        private static readonly string Admin = "admin";
        private static readonly string User = "user";

        private static IdentityRole[] _roles;
        private static ApplicationUser[] _admins;
        private static ApplicationUser[] _users;
        private static ApplicationDbContext _context;
        private static UserManager<ApplicationUser> _userManager;

        static SampleData()
        {
            _roles = new IdentityRole[]
            {
                new IdentityRole() { Name = Admin },
                new IdentityRole() { Name = User }
            };

            _admins = new ApplicationUser[]
            {
                new ApplicationUser()
                {
                    UserName = "admin@somedomain.com",
                    Email = "admin@somedomain.com",
                },
                new ApplicationUser()
                {
                    UserName = "admin2@somedomain.com",
                    Email = "admin2@somedomain.com",
                }
            };

            _users = new ApplicationUser[]
            {
                new ApplicationUser()
                {
                    UserName = "user1@somedomain.com",
                    Email = "user1@somedomain.com",
                },
                new ApplicationUser()
                {
                    UserName = "user2@somedomain.com",
                    Email = "user2@somedomain.com",
                }
            };
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            _context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            _userManager = (UserManager<ApplicationUser>)serviceProvider.GetRequiredService(
                typeof(UserManager<ApplicationUser>));

            await CreateRoles();
            await CreateUsers(_admins, Admin);
            await CreateUsers(_users, User);

            _context.SaveChanges();
        }


        private static async Task CreateRoles()
        {
            foreach (var role in _roles)
                if (!_context.Roles.Any(x => x.Name.Equals(role.Name)))
                    _context.Roles.Add(role);
        }

        private static async Task CreateUsers(
            ApplicationUser[] users,
            string role,
            string password = "CegthGfhjkm)*6")
        {
            foreach (var user in users)
            {
                var existingAdminUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingAdminUser != null)
                {
                    if (!(await _userManager.IsInRoleAsync(existingAdminUser, role)))
                        await _userManager.AddToRoleAsync(existingAdminUser, role);
                }
                else
                {
                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, role);
                }
            }


        }
    }
}
