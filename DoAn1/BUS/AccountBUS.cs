using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.BUS
{
    internal class AccountBUS
    {
        public static string GetSavedAccountPassword()
        {
            var passwordInConfig = DoAn1.Properties.Settings.Default.Password;
            // var passwordInByte = Encoding.UTF8.GetBytes(passwordInConfig);
            var entropy = DoAn1.Properties.Settings.Default.AccountPasswordEntropy;

            if (string.IsNullOrEmpty(passwordInConfig) || string.IsNullOrEmpty(entropy)) return "";

            byte[] savedPasswordBytes = Convert.FromBase64String(passwordInConfig);
            byte[] savedEntropy = Convert.FromBase64String(entropy);

            byte[] decryptedPassword = ProtectedData.Unprotect(savedPasswordBytes, savedEntropy, DataProtectionScope.CurrentUser);
            var savedPassword = Encoding.UTF8.GetString(decryptedPassword);

            return savedPassword;
        }

        public static void SaveAccountPassword(string password)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new Byte[20];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);

            DoAn1.Properties.Settings.Default.Password = Convert.ToBase64String(cypherText);
            DoAn1.Properties.Settings.Default.AccountPasswordEntropy = Convert.ToBase64String(entropy);

            DoAn1.Properties.Settings.Default.Save();
        }
    }
}
