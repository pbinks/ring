using System;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace Kerv.Common
{
    public class Credentials
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string Username
        {
            get => AppSettings.GetValueOrDefault(nameof(Username), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Username), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static void Clear() {
            AppSettings.Remove(nameof(Username));
            AppSettings.Remove(nameof(Password));
        }
    }
}
