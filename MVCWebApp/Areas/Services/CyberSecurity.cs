using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVCWebApp.Areas.Services
{
    public class CyberSecurity : ICyberSecurity
    {
        private readonly string _keyValue = "d41f7f2e392d4aa4b649fc702dd277d9";

        public string GetConnectionString(string value)
        {
            //extract password of conn string and decrypt then re-assemble conn string
            string[] strArr = value.Split(';');   //"password=4321Grimlyblobbby"
            var ndx = strArr[3].IndexOf('=');
            var strTemp = strArr[3].Substring(ndx + 1);  //this is the encrypted password

            strArr[3] = strArr[3].Substring(0, ndx + 1); //"password=";

            //for (int i = strTemp.Length - 1; i >= 0; i--) 
            strArr[3] += Decrypt(strTemp);//strTemp[i];

            return string.Join(';', strArr);
        }
        public string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            try
            {
                value = value.Replace(" ", "+");
                var fullCipher = Convert.FromBase64String(value);

                var iv = new byte[16];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
                var key = Encoding.UTF8.GetBytes(_keyValue);

                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                    {
                        string result;
                        using (var msDecrypt = new MemoryStream(cipher))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public Task<string> Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value)) return Task.FromResult(value);
            try
            {
                var key = Encoding.UTF8.GetBytes(_keyValue);

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(value);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            var str = Convert.ToBase64String(result);
                            var fullCipher = Convert.FromBase64String(str);
                            return Task.FromResult(str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(string.Empty);
            }
        }

    }
}
