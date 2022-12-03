using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode
{
    public class SessionExtractor
    {
        public struct CookieData
        {
            public string CookieName { get; set; }
            public string Domain { get; set; }
            public string Value { get; set; }

            public CookieData(string cookieName, string domain, string value)
            {
                CookieName = cookieName;
                Domain = domain;
                Value = value;
            }
        }

        public const string HOST_KEY = ".adventofcode.com";
        public const string COOKIE_NAME = "session";
        public static CookieData GetAocSessinCookie()
        {

            string localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string chromeRootPath = localAppdata + @"\Google\Chrome\User Data\";
            string localStatePath = chromeRootPath + @"\Local State";
            string cookieDbPath = chromeRootPath + @"\Default\Network\Cookies";

            byte[] sessionCookie = GetAocSessionCookieFromChrome(cookieDbPath);
            var chromeDecryptKey = GetDecryptKey(localStatePath);

            string sessionKey = Decrypt(sessionCookie, chromeDecryptKey);
            int keyLen = sessionKey.Length;
            Console.WriteLine($"Session key found value='{sessionKey[0..8]}{new String('*', 8)}{sessionKey[(keyLen - 8)..(keyLen)]}'");

            return new CookieData(COOKIE_NAME, HOST_KEY, sessionKey);
        }

        public static void Prepare(byte[] encryptedData, out byte[] nonce, out byte[] ciphertextTag)
        {
            nonce = new byte[12];
            ciphertextTag = new byte[encryptedData.Length - 3 - nonce.Length];

            Array.Copy(encryptedData, 3, nonce, 0, nonce.Length);
            Array.Copy(encryptedData, 3 + nonce.Length, ciphertextTag, 0, ciphertextTag.Length);
        }

        public static string Decrypt(byte[] encryptedBytes, byte[] key)
        {
            Prepare(encryptedBytes, out byte[] nonce, out byte[] ciphertextTag);
            string sR;
            try
            {
                GcmBlockCipher cipher = new(new AesEngine());
                AeadParameters parameters = new(new KeyParameter(key), 128, nonce, null);

                cipher.Init(false, parameters);
                byte[] plainBytes = new byte[cipher.GetOutputSize(ciphertextTag.Length)];
                Int32 retLen = cipher.ProcessBytes(ciphertextTag, 0, ciphertextTag.Length, plainBytes, 0);
                cipher.DoFinal(plainBytes, retLen);

                sR = Encoding.UTF8.GetString(plainBytes).TrimEnd("\r\n\0".ToCharArray());
            }
            catch (Exception ex)
            {
                return $"Decryption failed :(\nError:{ex}";
            }
            return sR;
        }

        private static byte[] GetAocSessionCookieFromChrome(string cookieDbPath)
        {
            string connString = @"URI=file:" + cookieDbPath;
            SQLiteConnection con = new SQLiteConnection(connString);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = $"SELECT encrypted_value FROM Cookies WHERE host_key='{HOST_KEY}' and name='{COOKIE_NAME}'";
            var sessionKey = (byte[])cmd.ExecuteScalar();
            return sessionKey;
        }
        private static byte[] GetDecryptKey(string localStatePath)
        {
            var jsonData = File.ReadAllText(localStatePath);
            var localState = JsonConvert.DeserializeObject<dynamic>(jsonData)!;
            var masterKeyEncrypted = Convert.FromBase64String((string)localState.os_crypt.encrypted_key)[5..];
            var optionalEntropy = new byte[] { };
            var masterKeyDecrypted = ProtectedData.Unprotect(masterKeyEncrypted, optionalEntropy, DataProtectionScope.CurrentUser);
            return masterKeyDecrypted;
        }
    }
}
