using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/login/{userEmail}/{userPassword}", (string userEmail, string userPassword) =>
{
    var token = TokenHelper.GetToken(userEmail, userPassword);
    return new { Token = token };
});

// Navigate to '/login/pete.h@gmail.com/pass' to get and display token

app.Run();

public class Token
{
    public int UserId { get; set; }
    public DateTime Expires { get; set; }
}

public static class TokenHelper
{
    public static string GetToken(string userName, string password)
    {
        // do a db lookup to confirm the userName and password
        // create the token
        var token = new Token
        {
            UserId = 10,
            Expires = DateTime.UtcNow.AddMinutes(1),
        };
        var jsonString = JsonConvert.SerializeObject(token);
        var encryptedJsonString = Crypto.EncryptStringAES(jsonString);
        return encryptedJsonString;
    }

}

public class Crypto
{
    private static readonly byte[] Salt =
        Encoding.ASCII.GetBytes("B78A07A7-14D8-4890-BC99-9145A14713C1");
    private const string Password = "sharedSecretPassword";
    /// <summary>
    /// Encrypt the given string using AES.
    /// The string can be decrypted using DecryptStringAES().
    ///</summary>
    /// <param name="plainText">The text to encrypt.</param>
    public static string EncryptStringAES(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentNullException("plainText");
        }

        string outStr;                   // Encrypted string to return
        RijndaelManaged aesAlg = null;   // Used to encrypt the data
        try
        {
            var key = new Rfc2898DeriveBytes(Password, Salt);
            aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

            // Create a decryptor to perform the stream transform.
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                // prepend the IV
                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                outStr = Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        finally
        {
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
            {
                aesAlg.Clear();
            }
        }
        // Return the encrypted bytes from the memory stream.
        return outStr;
    }
}
