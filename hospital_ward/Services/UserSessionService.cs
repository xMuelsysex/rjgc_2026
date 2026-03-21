using System;
using System.Linq;
using MyFirstApp.Models;
using MyFirstApp.ViewModels;

namespace MyFirstApp.Services;

public class UserSessionService
{
    private static readonly Lazy<UserSessionService> _instance = new(() => new UserSessionService());
    public static UserSessionService Instance => _instance.Value;

    private readonly DataService _dataService = DataService.Instance;

    public UserCredential? CurrentCredential { get; private set; }

    public UserProfile? CurrentProfile => CurrentCredential is null
        ? null
        : _dataService.UserProfiles.FirstOrDefault(x => x.Role == CurrentCredential.Role);

    public SystemRole CurrentRole => Enum.TryParse<SystemRole>(CurrentCredential?.Role, out var role)
        ? role
        : SystemRole.Admin;

    public bool Login(string username, string password, out string message)
    {
        var credential = _dataService.UserCredentials.FirstOrDefault(x => x.Username == username);
        if (credential is null)
        {
            message = "账号不存在。";
            return false;
        }

        if (credential.Password != password)
        {
            message = "密码不正确。";
            return false;
        }

        CurrentCredential = credential;
        message = "登录成功。";
        return true;
    }

    public void Logout()
    {
        CurrentCredential = null;
    }
}
