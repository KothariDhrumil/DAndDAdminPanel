using Application.Identity.User;

namespace Application.Identity.Tokens;

public interface IUserService : ITransientService
{
    Task<UserViewModel> GetUserDetailsAsync(string UserId);
}

