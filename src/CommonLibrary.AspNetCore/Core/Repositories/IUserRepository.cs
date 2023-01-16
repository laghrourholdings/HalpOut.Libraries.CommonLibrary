using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Core;

public interface IUserRepository<T> : IRepository<T>
{
    public Task SignOutAsync(T user);
    public Task SignInAsync(T user, bool isPersistent = false);
}