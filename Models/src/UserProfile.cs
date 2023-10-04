namespace WebApp.Models;

// Partial class
public partial class WebApp {

    #pragma warning disable 169
    /// <summary>
    /// UserProfile class
    /// </summary>
    public class UserProfile
    {
        public string Username = "";

        private Dictionary<string, string> _profile = new ();

        public int TimeoutTime;

        public int MaxRetryCount;

        public int RetryLockoutTime;

        public int PasswordExpiryTime;

        private string BackupUsername = "";

        private Dictionary<string, string> _backupProfile = new ();

        private Dictionary<string, string> _allfilters = new ();

        // Constructor
        public UserProfile()
        {
            // Max login retry
            SetValue(Config.UserProfileLoginRetryCount, "0");
            SetValue(Config.UserProfileLastBadLoginDateTime, "");
            MaxRetryCount = Config.UserProfileMaxRetry;
            RetryLockoutTime = Config.UserProfileRetryLockout;
            Load();
        }

        // Contains key // DN
        public bool ContainsKey(string name) => _profile.ContainsKey(name);

        // Get value
        public string GetValue(string name) => _profile.TryGetValue(name, out string? value) ? value : "";

        // Set value
        public void SetValue(string name, string value) => _profile[name] = value;

        // Get/Set as string
        public string this[string name]
        {
            get => GetValue(name);
            set => SetValue(name, value);
        }

        // Try get value
        public bool TryGetValue(string name, [NotNullWhen(true)] out string? value) => _profile.TryGetValue(name, out value);

        // Delete property
        public void Remove(string name) => _profile.Remove(name);

        // Backup profile
        public void Backup(string user)
        {
            if (!Empty(Username) && user != Username) {
                BackupUsername = Username;
                _backupProfile = new (_profile);
            }
        }

        // Restore profile
        public void Restore(string user)
        {
            if (!Empty(BackupUsername) && user != BackupUsername) {
                Username = BackupUsername;
                _profile = new (_backupProfile);
            }
        }

        // Assign properties
        public void Assign(Dictionary<string, object>? input)
        {
            if (input == null)
                return;
            foreach (var (key, value) in input) {
                if (ContainsKey(key))
                    continue;
                if (IsNull(value))
                    SetValue(key, "");
                else if (value is bool || IsNumeric(value))
                    SetValue(key, ConvertToString(value));
                else if (value is string str && str.Length <= Config.DataStringMaxLength)
                    SetValue(key, str);
            }
        }

        // Assign properties
        public void Assign(Dictionary<string, string>? input)
        {
            if (input == null)
                return;
            foreach (var (key, value) in input)
                SetValue(key, value);
        }

        // Check if System Admin
        protected bool IsSysAdmin(string user) {
            string adminUserName = Config.EncryptionEnabled ? AesDecrypt(Config.AdminUserName) : Config.AdminUserName;
            return (user == "" || user == adminUserName);
        }

        // Get language id
        public async Task<string> GetLanguageId(string user)
        {
            try {
                if (await LoadProfileFromDatabase(user))
                    return GetValue(Config.UserProfileLanguageId);
                return "";
            } catch {
                if (Config.Debug)
                    throw;
                return "";
            } finally {
                Restore(user); // Restore current profile
            }
        }

        // Set language id
        public async Task<bool> SetLanguageId(string user, string langid)
        {
            try {
                if (await LoadProfileFromDatabase(user)) {
                    SetValue(Config.UserProfileLanguageId, langid);
                    return await SaveProfileToDatabase(user);
                }
                return false;
            } catch {
                if (Config.Debug)
                    throw;
                return false;
            } finally {
                Restore(user); // Restore current profile
            }
        }

        // Get search filters
        public async Task<string> GetSearchFilters(string user, string pageid)
        {
            try {
                if (await LoadProfileFromDatabase(user)) {
                    string searchFilters = GetValue(Config.UserProfileSearchFilters);
                    if (!Empty(searchFilters)) {
                        _allfilters = StringToProfile(searchFilters);
                        if (_allfilters.TryGetValue(pageid, out string? result))
                            return result;
                    }
                }
                return "";
            } catch {
                if (Config.Debug)
                    throw;
                return "";
            } finally {
                Restore(user); // Restore current profile
            }
        }

        // Set search filters
        public async Task<bool> SetSearchFilters(string user, string pageid, string filters)
        {
            try {
                if (await LoadProfileFromDatabase(user)) {
                    string searchFilters = GetValue(Config.UserProfileSearchFilters);
                    if (!Empty(searchFilters))
                        _allfilters = StringToProfile(searchFilters);
                    if (!_allfilters.ContainsKey(pageid))
                        _allfilters.Add(pageid, filters);
                    else
                        _allfilters[pageid] = filters;
                    SetValue(Config.UserProfileSearchFilters, ConvertToJson(_allfilters));
                    return await SaveProfileToDatabase(user);
                }
                return false;
            } catch {
                if (Config.Debug)
                    throw;
                return false;
            } finally {
                Restore(user); // Restore current profile
            }
        }

        // Load profile from database
        public async Task<bool> LoadProfileFromDatabase(string user)
        {
            if (IsSysAdmin(user)) // Ignore system admin
                return false;
            else if (user == Username) // Already loaded, skip
                return true;

            // Ignore hard code admin
            UserTable = Resolve("usertable")!;
            UserTableConn = GetConnection(UserTable.DbId);
            string filter = GetUserFilter(Config.LoginUsernameFieldName, user);
            // Get SQL from GetSql method in <UserTable> class
            string sql = UserTable.GetSql(filter); // DN
            try {
                var row = await UserTableConn.GetRowAsync(sql);
                if (row != null) {
                    Backup(user); // Backup user profile if exists
                    Clear();
                    string p = HtmlDecode(ConvertToString(row[Config.UserProfileFieldName]));
                    LoadProfile(p);
                    Username = user;
                    return true;
                }
                return false;
            } catch {
                if (Config.Debug)
                    throw;
                return false;
            }
        }

        // Save profile to database
        public async Task<bool> SaveProfileToDatabase(string user)
        {
            if (IsSysAdmin(user)) // Ignore system admin
                return true;
            UserTable = Resolve("usertable")!;
            var row = new Dictionary<string, object> { { Config.UserProfileFieldName, ProfileToString() } };
            var filter = new Dictionary<string, object> { { Config.LoginUsernameFieldName, user } };
            return await UserTable.UpdateAsync(row, filter) > 0;
        }

        // Load profile from session
        public void Load() => LoadProfile(Session.GetString(Config.SessionUserProfile));

        // Save profile to session
        public void Save() => Session[Config.SessionUserProfile] = ProfileToString();

        // Load profile
        public void LoadProfile(string str)
        {
            if (!Empty(str) && !SameString(str, "{}")) // DN
                _profile = StringToProfile(str);
        }

        // Clear profile
        public void ClearProfile() => _profile.Clear();

        // Clear profile (alias)
        public void Clear() => ClearProfile();

        // Serialize profile
        public string ProfileToString() => JsonConvert.SerializeObject(_profile);

        // Split profile
        private Dictionary<string, string> StringToProfile(string str)
        {
            try {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(str) ?? new ();
            } catch {
                return new ();
            }
        }

        // Exceed login retry
        public async Task<bool> ExceedLoginRetry(string user)
        {
            if (IsSysAdmin(user)) // Ignore system admin
                return false;
            try {
                if (await LoadProfileFromDatabase(user)) {
                    int retrycount = ConvertToInt(GetValue(Config.UserProfileLoginRetryCount));
                    string dt = GetValue(Config.UserProfileLastBadLoginDateTime);
                    if (retrycount >= MaxRetryCount) {
                        if (DateTime.Compare(DateTime.Parse(dt).AddMinutes(RetryLockoutTime), DateTime.Now) > 0) {
                            return true;
                        } else {
                            SetValue(Config.UserProfileLoginRetryCount, "0");
                            await SaveProfileToDatabase(user);
                            return false;
                        }
                    }
                }
                return false;
            } catch {
                if (Config.Debug)
                    throw;
                return false;
            } finally {
                Restore(user); // Restore current profile
            }
        }

        // Reset login retry
        public async Task<bool> ResetLoginRetry(string user)
        {
            try {
                if (await LoadProfileFromDatabase(user)) {
                    SetValue(Config.UserProfileLoginRetryCount, "0");
                    return await SaveProfileToDatabase(user);
                }
                return false;
            } catch {
                if (Config.Debug)
                    throw;
                return false;
            } finally {
                Restore(user); // Restore current profile
            }
        }

        #pragma warning disable 1998

        // User has 2FA secret
        public async Task<bool> HasUserSecret(string user, bool verified = false) => false;

        // Verify 2FA code
        public async Task<bool> Verify2FACode(string user, string code) => false;

        // Get User 2FA secret
        public async Task<string> GetUserSecret(string user) => "";

        // Set one time passwword (Email/SMS)
        public async Task<bool> SetOneTimePassword(string user, string account, string otp) => false;

        // Get backup codes
        public async Task<List<string>> GetBackupCodes(string user = "") => new ();

        // Get new set of backup codes
        public async Task<List<string>> GetNewBackupCodes(string user) => new ();

        // Reset user secret
        public async Task<bool> ResetUserSecret(string user) => false;
        #pragma warning restore 1998
    }
    #pragma warning restore 169
} // End Partial class
