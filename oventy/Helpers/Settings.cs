using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace oventy
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string UsernameKey = "username";
        private static readonly string UsernameDefault = string.Empty;

        private const string PasswordKey = "password";
        private static readonly string PasswordDefault = string.Empty;

        private const string RememberMeKey = "remember_me";
        private static readonly bool RememberMeDefault = false;

        private const string AccessTokenKey = "access_token_key";
        private static readonly string TokenDefault = string.Empty;

        private const string RefreshTokenKey = "refresh_token_key";

        private const string AccessTokenExpireTimeKey = "access_toke_expire_time_key";
        private static readonly DateTime AccessTokenExpireTimeDefault = DateTime.UtcNow;
        
        #endregion


        public static string Username
        {
            get { return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault); }
            set { AppSettings.AddOrUpdateValue(UsernameKey, value); }
        }

        public static string Password
        {
            get { return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault); }
            set { AppSettings.AddOrUpdateValue(PasswordKey, value); }
        }

        public static bool RememberMe
        {
            get { return AppSettings.GetValueOrDefault(RememberMeKey, RememberMeDefault); }
            set { AppSettings.AddOrUpdateValue(RememberMeKey, value); }
        }

        public static string AccessToken
        {
            get { return AppSettings.GetValueOrDefault(AccessTokenKey, TokenDefault); }
            set { AppSettings.AddOrUpdateValue(AccessTokenKey, value); }
        }

        public static string RefreshToken
        {
            get { return AppSettings.GetValueOrDefault(RefreshTokenKey, TokenDefault); }
            set { AppSettings.AddOrUpdateValue(RefreshTokenKey, value); }
        }

        public static DateTime AccessTokenExpireTime
        {
            get { return AppSettings.GetValueOrDefault(AccessTokenExpireTimeKey, DateTime.UtcNow); }
            set { AppSettings.AddOrUpdateValue(AccessTokenExpireTimeKey, value); }
        }

        public static void RemoveUsername()
        {
            AppSettings.Remove(UsernameKey);
        }

        public static void RemovePassword()
        {
            AppSettings.Remove(PasswordKey);
        }

        public static void RemoveAccessToken()
        {
            AppSettings.Remove(AccessTokenKey);
        }

        public static void RemoveAccessTokenExpireTime()
        {
            AppSettings.Remove(AccessTokenExpireTimeKey);
        }

    }
}
