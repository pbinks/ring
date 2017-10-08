using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Kerv.Common
{
    public class Account
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool IsBalanceSet => AppSettings.Contains(nameof(Balance));

        public static Money Balance {
            get => new Money(AppSettings.GetValueOrDefault(nameof(Balance), 0));
            set => AppSettings.AddOrUpdateValue(nameof(Balance), value.Pence);
        }
    }
}
