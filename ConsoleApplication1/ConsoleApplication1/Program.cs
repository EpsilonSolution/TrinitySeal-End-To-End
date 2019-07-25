using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static string keys { get; set; }
        static string salt { get; set; }
        public static string Payload_DECRYPT(string value)
        {
            string message = value;
            string password = Encoding.Default.GetString(Convert.FromBase64String(keys));
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));
            byte[] iv = Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(salt)));
            string decrypted = String_Encryption.DecryptString(message, key, iv);
            return decrypted;
        }
        public static string Payload_ENCRYPT(string value)
        {
            string message = value;
            string password = Encoding.Default.GetString(Convert.FromBase64String(keys));
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));
            byte[] iv = Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(salt)));
            string decrypted = String_Encryption.EncryptString(message, key, iv);
            return decrypted;
        }
        static void Main(string[] args)
        {
            keys = "UTY4UVd1RWJicTBwQzFtMGxxNk9KSm9PUWJXdWFDSUg=";
            salt = "R3VDM0hCbkkyWDM3V09Sbg==";
            Console.WriteLine(Payload_DECRYPT("sFOFbkiNDB5JL2dwxMHr9g3I1SLFKYREn2sEI4TJEHV9IQnRaI0W8sqGeLIk6Xiolo6elLWgikupOkcy22aWklCVxoXUV8aqNCtLLq6mVvD5m2fsMkaap5KU34a1mwdXbfWPlUTh9+m8GXuWgwkrHyLcnXRDrGYzjTPg0kHlto6jkBoHYXpL1+6F1hWmKrdy2e0Cy9KTR2mA1oADG9wQvvEao7O82cX4ggKmi8iLb1zs8W7IkWF3XPhYAn6bYpBFfj7jbn6bFewWu8w+CI4xsJoyLWoxp4qxCUrRkvlKerG5uVhy40iMi+eEtkHmFuBV"));
            Console.Read();
        }
    }
    class String_Encryption
    {
        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
            string plainText = String.Empty;
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] plainBytes = memoryStream.ToArray();
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainText;
        }
        public static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            return cipherText;
        }

        
    }
}
