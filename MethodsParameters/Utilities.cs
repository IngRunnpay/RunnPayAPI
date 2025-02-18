using Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters
{
    public static class Utilities
    {
        public static readonly string Invalid = "Petición no valida.";

        public static class OperacionEncriptacion
        {
            public static class Keys
            {
                public static readonly string UserAgreementKEY = "UserAgreementKEY";
                public static readonly string UserAgreementIV = "UserAgreementIV";
                public static readonly string UserValidationKEY = "UserAgreementKEY";
                public static readonly string UserValidationIV = "UserAgreementIV";
                public static readonly string DIANValueKEY = "DIANVALUEKEY";
                public static readonly string DIANValueIV = "DIANVALUEIV";
            }

            public static string Generate_8()
            {
                return Generate(8);
            }
            public static string Generate_4()
            {
                return Generate(4);
            }
            private static string Generate(int length)
            {
                var characters = "0123456789";
                var Charsarr = new char[length];
                var random = new Random();

                for (int i = 0; i < Charsarr.Length; i++)
                {
                    Charsarr[i] = characters[random.Next(characters.Length)];
                }

                return EncryptString(new string(Charsarr), string.Empty, string.Empty);
            }

            public static string EncryptString(string message, string KeyString, string IVString)
            {
                byte[] Key = Encoding.UTF8.GetBytes(KeyString);
                byte[] IV = Encoding.UTF8.GetBytes(IVString);

                string encrypted = null;
                RijndaelManaged rj = new RijndaelManaged();
                int keySize = 32;
                int ivSize = 16;
                Array.Resize(ref Key, keySize);
                Array.Resize(ref IV, ivSize);
                rj.Key = Key;
                rj.IV = IV;

                rj.Mode = CipherMode.CBC;

                try
                {
                    MemoryStream ms = new MemoryStream();

                    using (CryptoStream cs = new CryptoStream(ms, rj.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(message);
                            sw.Close();
                        }
                        cs.Close();
                    }
                    byte[] encoded = ms.ToArray();
                    encrypted = Convert.ToBase64String(encoded);

                    ms.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    rj.Clear();
                }
                return encrypted;
            }

            public static string DecryptAES(string cipherData, string keyString, string ivString)
            {
                byte[] key = Encoding.UTF8.GetBytes(keyString);
                byte[] iv = Encoding.UTF8.GetBytes(ivString);
                int keySize = 32;
                int ivSize = 16;
                Array.Resize(ref key, keySize);
                Array.Resize(ref iv, ivSize);

                try
                {
                    using (var rijndaelManaged =
                           new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
                    using (var memoryStream =
                           new MemoryStream(Convert.FromBase64String(cipherData)))
                    using (var cryptoStream =
                           new CryptoStream(memoryStream,
                               rijndaelManaged.CreateDecryptor(key, iv),
                               CryptoStreamMode.Read))
                    {
                        return new StreamReader(cryptoStream).ReadToEnd();
                    }
                }
                catch
                {
                    throw;
                }
            }

            /// <summary>
            /// Codificar el valor de un string
            /// </summary>
            /// <param name="inputStr">string a codificar</param>        
            /// <returns>string codificado</returns>
            public static string Base64Encode(string inputStr)
            {
                if (string.IsNullOrEmpty(inputStr))
                    return inputStr;

                var encodedStr = Encoding.UTF8.GetBytes(inputStr);
                return Convert.ToBase64String(encodedStr);
            }

            /// <summary>
            /// Descodificar el valor de un string
            /// </summary>
            /// <param name="inputStr">string a descodificar</param>        
            /// <returns>string descodificado</returns>
            public static string Base64Decode(string inputStr)
            {
                if (string.IsNullOrEmpty(inputStr))
                    return inputStr;

                var decodedStr = Convert.FromBase64String(inputStr);
                return Encoding.UTF8.GetString(decodedStr);
            }

        }

        public static class OperacionPasarelasExternas
        {
            public static int GetIdTransaccionByReferencia(string Referencia)
            {
                int IdTransaccion = 0;
                string[] Id = Referencia.Split('|');
                IdTransaccion = Convert.ToInt32(Id[1]);

                return IdTransaccion;
            }
            public static string FormatearFecha(string fecha)
            {
                DateTime fechaConvertida = DateTime.Parse(fecha);
                return fechaConvertida.ToString("yyyy-MM-dd");
            }
        }
    }
}
