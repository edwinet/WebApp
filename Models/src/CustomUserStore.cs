namespace WebApp.Models;

/// <summary>
/// Custom user store class
/// </summary>
public class CustomUserStore : IUserStore<ApplicationUser>,
    IUserPasswordStore<ApplicationUser>
{
    // Constructor
    public CustomUserStore()
    {
        UserTable = Resolve("usertable")!;
        UserTableConn = GetConnection(UserTable.DbId);
    }

    // CreateAsync
    public Task<IdentityResult> CreateAsync(ApplicationUser user,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        throw new NotImplementedException();

    // DeleteAsync
    public Task<IdentityResult> DeleteAsync(ApplicationUser user,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        throw new NotImplementedException();

    // Dispose
    public void Dispose() {}

    // FindByIdAsync
    public async Task<ApplicationUser> FindByIdAsync(string userId,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Empty(userId))
            throw new ArgumentNullException(nameof(userId));
        string filter = Config.UserIdFilter.Replace("%u", AdjustSql(userId, Config.UserTableDbId));
        string sql = UserTable.GetSql(filter);
        var user = await UserTableConn.GetRowAsync(sql);
        return user != null
            ? new ApplicationUser {
                    Id = ConvertToString(user[Config.UserIdFieldName]),
                    UserName = ConvertToString(user[Config.LoginUsernameFieldName]),
                    Email = !Empty(Config.UserEmailFieldName) ? ConvertToString(user[Config.UserEmailFieldName]) : "",
                    ParentUserId = !Empty(Config.ParentUserIdFieldName) ? ConvertToString(user[Config.ParentUserIdFieldName]) : "",
                    UserLevelId = !Empty(Config.UserLevelFieldName) ? ConvertToString(user[Config.UserLevelFieldName]) : "",
                    IsAuthenticated = true
                }
            : new ApplicationUser();
    }

    // FindByNameAsync
    public async Task<ApplicationUser> FindByNameAsync(string userName,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (userName == null)
            throw new ArgumentNullException(nameof(userName));
        string filter = Config.UserNameFilter.Replace("%u", AdjustSql(userName, Config.UserTableDbId));
        string sql = UserTable.GetSql(filter);
        var user = await UserTableConn.GetRowAsync(sql);
        return user != null
            ? new ApplicationUser {
                    Id = ConvertToString(user[Config.UserIdFieldName]),
                    UserName = ConvertToString(user[Config.LoginUsernameFieldName]),
                    Email = !Empty(Config.UserEmailFieldName) ? ConvertToString(user[Config.UserEmailFieldName]) : "",
                    ParentUserId = !Empty(Config.ParentUserIdFieldName) ? ConvertToString(user[Config.ParentUserIdFieldName]) : "",
                    UserLevelId = !Empty(Config.UserLevelFieldName) ? ConvertToString(user[Config.UserLevelFieldName]) : "",
                    IsAuthenticated = true
                }
            : new ApplicationUser();
    }

    // GetNormalizedUserNameAsync
    public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // GetPasswordHashAsync
    public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // GetUserIdAsync
    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        return Task.FromResult(user.Id);
    }

    // GetUserNameAsync
    public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        return Task.FromResult<string>(user.UserName);
    }

    // HasPasswordAsync
    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // SetNormalizedUserNameAsync
    public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // SetPasswordHashAsync
    public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // SetUserNameAsync
    public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // UpdateAsync
    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
