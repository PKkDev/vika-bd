﻿using System.Security.Cryptography;
using System.Text;

internal class Program
{
    /// <summary>
    /// https://medium.com/@cemalcanakgul/integrating-aes-encryption-in-c-a-developers-guide-837f2079448a
    /// </summary>
    /// <param name="args"></param>
    private static void Main(string[] args)
    {
        var keyStr = "";

        var strings = new List<string>()
        {
            "Кирилл",
            "Семья Митиогло",
            "Алена",
            "Денис",
            "Полина",
            "Гриша",
            "Семья Буслаевых",
            "Семья Цой",
        };

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = CreateAesKey(keyStr);

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        foreach (var item in strings)
        {
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(item);
            }

            var encrypted = msEncrypt.ToArray();
            var encryptedStr = Convert.ToBase64String(encrypted);
            //var encryptedStrUrl = System.Net.WebUtility.UrlEncode(encryptedStr);
            //var encryptedStrUrlSafe = Uri.EscapeDataString(encryptedStr);
            var encryptedStrUrlSafeV2 = UrlEncode(encryptedStr);

            Console.WriteLine($"Для {item}");
            Console.WriteLine($"encryptedStr: {encryptedStr}");
            //Console.WriteLine($"encryptedStrUrl: {encryptedStrUrl}");
            //Console.WriteLine($"encryptedStrUrlSafe: {encryptedStrUrlSafe}");
            Console.WriteLine($"encryptedStrUrlSafeV2: {encryptedStrUrlSafeV2}");
            Console.WriteLine();
        }
    }

    public static byte[] CreateAesKey(string inputString)
    {
        List<byte> res = new();
        foreach (var item in inputString)
            res.Add(Convert.ToByte(item));
        return res.ToArray();

        return Encoding.UTF8.GetByteCount(inputString) == 32
            ? Encoding.UTF8.GetBytes(inputString)
            : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    private readonly static string reservedCharacters = "!*'();:@&=+$,/?%#[]";

    public static string UrlEncode(string value)
    {
        if (String.IsNullOrEmpty(value))
            return String.Empty;

        var sb = new StringBuilder();

        foreach (char @char in value)
        {
            if (reservedCharacters.IndexOf(@char) == -1)
                sb.Append(@char);
            //else
            //    sb.AppendFormat("%{0:X2}", (int)@char);
        }
        return sb.ToString();
    }

}


