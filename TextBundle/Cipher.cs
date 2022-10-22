using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TextBundle
{
    public class Cipher
    {
        private static readonly string DefaultKey = "mYq3t6v9y$B&E)H@McQfTjWnZr4u7x!z";
        private static readonly string DefaultIV = "TjWnZr4u7x!A%D*G-KaNdRgUkXp2s5v8";

        public static byte[] Encrypt(byte[] input)
        {
            var encryptor = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateEncryptor(Encoding.UTF8.GetBytes(DefaultKey), Encoding.UTF8.GetBytes(DefaultIV));

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] input)
        {
            var decryptor = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateDecryptor(Encoding.UTF8.GetBytes(DefaultKey), Encoding.UTF8.GetBytes(DefaultIV));

            using (var memoryStream = new MemoryStream(input))
            {
                using (var decryptStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptStream);
                    }
                    decryptStream.Position = 0;
                    return decryptStream.ToArray();
                }
            }
        }
    }
}
