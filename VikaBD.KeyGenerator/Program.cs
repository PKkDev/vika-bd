using System.Security.Cryptography;
using System.Text;

internal class Program
{
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
            var encryptedStrUrl = System.Net.WebUtility.UrlEncode(encryptedStr);

            Console.WriteLine($"Для {item}");
            Console.WriteLine($"encryptedStr: {encryptedStr}");
            Console.WriteLine($"encryptedStrUrl: {encryptedStrUrl}");
            Console.WriteLine();
        }
    }

    public static byte[] CreateAesKey(string inputString)
    {
        var asd = Encoding.UTF8.GetByteCount(inputString);

        return Encoding.UTF8.GetByteCount(inputString) == 32
            ? Encoding.UTF8.GetBytes(inputString)
            : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

}


