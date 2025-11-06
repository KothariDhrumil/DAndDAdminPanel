using Application.Identity.Tokens;
using Application.Identity.User;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    internal class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserViewModel> GetUserDetailsAsync(string UserId)
        {

            return await _userManager.Users
                                      .AsNoTracking()
                                     .Where(x => x.Id == UserId)
                                     .Select(user =>
                                     new UserViewModel()
                                     {
                                         FirstName = user.FirstName,
                                         LastName = user.LastName
                                     }).SingleOrDefaultAsync();
        }
    }
}
