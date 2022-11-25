using CommonLibrary.Core;
using Microsoft.AspNetCore.Identity;

namespace CommonLibrary.AspNetCore;

public interface IUserRepository<T> : IRepository<T>
{
    public Task SignOutAsync(T user);
    public Task SignInAsync(T user, bool isPersistent = false);
}