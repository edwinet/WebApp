namespace WebApp.Models;

/// <summary>
/// Custom role store class (Use User Level as role)
/// </summary>
public class CustomRoleStore : IRoleStore<ApplicationRole>
{
    private Roles _table;

    private DatabaseConnectionBase<SqlConnection, SqlCommand, SqlDataReader, SqlDbType> _conn;

    // Constructor
    public CustomRoleStore()
    {
        _table = Resolve(Config.UserLevelTableName) ?? throw new Exception($"Failed to resolve user table '{Config.UserLevelTableName}'");
        _conn = GetConnection(Config.UserLevelDbId);
    }

    // CreateAsync
    public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // DeleteAsync
    public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // Dispose
    public void Dispose() {}

    // FindByIdAsync
    public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Empty(roleId))
            throw new ArgumentNullException(nameof(roleId));
        string sql = _table.GetSql(Config.UserLevelIdField + " = "+ AdjustSql(roleId, Config.UserLevelDbId));
        var role = await _conn.GetRowAsync(sql);
        return new ApplicationRole {
            Id = ConvertToString(role?[Config.UserLevelIdFieldName]),
            Name = ConvertToString(role?[Config.UserLevelNameFieldName])
        };
    }

    // FindByNameAsync
    public async Task<ApplicationRole> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Empty(roleName))
            throw new ArgumentNullException(nameof(roleName));
        string sql = _table.GetSql(Config.UserLevelNameField + " = "+ AdjustSql(roleName, Config.UserLevelDbId));
        var role = await _conn.GetRowAsync(sql);
        return new ApplicationRole {
            Id = ConvertToString(role?[Config.UserLevelIdFieldName]),
            Name = ConvertToString(role?[Config.UserLevelNameFieldName])
        };
    }

    // GetNormalizedRoleNameAsync
    public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // GetRoleIdAsync
    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
            throw new ArgumentNullException(nameof(role));
        return Task.FromResult(role.Id);
    }

    // GetRoleNameAsync
    public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
            throw new ArgumentNullException(nameof(role));
        return Task.FromResult<string>(role.Name);
    }

    // SetNormalizedRoleNameAsync
    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // SetRoleNameAsync
    public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    // UpdateAsync
    public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
