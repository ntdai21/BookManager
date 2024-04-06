using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using Firebase.Database.Query;
using static DoAn1.BUS.FirebaseBUS;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;

namespace DoAn1.BUS
{
    public class FirebaseBUS
    {

        #region Singleton
        private static FirebaseBUS instance;
        public static FirebaseBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FirebaseBUS();
                }
                return instance;
            }
        }

        private FirebaseBUS() {
            // Configure...
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyAOpm9dK3MdwQX_rwLux4IOFFtYTcIZ4QM",
                AuthDomain = "bookmanager-fbfe2.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                // Add and configure individual providers
                new EmailProvider()
                    // ...
                },
                // WPF:
                UserRepository = new FileUserRepository("BookManager") // persist data into %AppData%\FirebaseSample
            };

            client = new FirebaseAuthClient(config);
        }
        #endregion

        FirebaseAuthClient client;
        UserCredential userCredential;
        FirebaseClient firebase = new FirebaseClient("https://bookmanager-fbfe2-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public struct LoginResult
        {
            public enum LoginResultType
            {
                Failed,
                Activated,
                NotActivated
            }

            public LoginResultType Result;
            public int DayRemain;

            public LoginResult(LoginResultType result,  int dayRemain)
            {
                Result = result;
                DayRemain = dayRemain;
            }

        }

        public async Task<LoginResult> Login(string email, string password)
        {
            try
            {
                userCredential = await Task.Run(() => {
                    try
                    {
                        var _userCredential = client.SignInWithEmailAndPasswordAsync(email, password);
                        return _userCredential;
                    }

                    catch (Exception)
                    {
                        return null;
                    }
                });

                if (userCredential != null)
                {
                    //Validation Account
                    var obj = await firebase.Child("Accounts").Child(userCredential.User.Uid).Child("ExpirationDate").OnceAsJsonAsync();

                    var expDateStr = obj.ToString().Replace("\"", "").Trim();

                    if (string.IsNullOrEmpty(expDateStr))
                    {

                        return new LoginResult(LoginResult.LoginResultType.Activated, -1);

                    }
                    else //Not activated yet
                    {
                        DateTime dt = DateTime.ParseExact(expDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        int daysRemaining = int.Max((dt.Date - DateTime.Now.Date).Days, 0);

                        return new LoginResult(LoginResult.LoginResultType.NotActivated, daysRemaining);
                    }
                }

            }
            catch (Exception ex)
            {
                return new LoginResult(LoginResult.LoginResultType.Failed, 0);
            }

            return new LoginResult(LoginResult.LoginResultType.Failed, 0);
        }

        public async Task<bool> ActivateAccount(string activationKey)
        {
            var obj = await firebase.Child("ActivationKeys").Child(activationKey).OnceAsJsonAsync();

            if (obj != "true") return false;
            else
            {
                await firebase.Child("Accounts").Child(userCredential.User.Uid).Child("ExpirationDate").PutAsync("\"\"");
            }

            return true;
        }
    }
}
