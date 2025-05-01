
using System.Security.Cryptography;

namespace firstProject.Services;

public class PasswordService{
    const string SALT = "adfyuj4545tgvhbf45";
    // public static string GenerateSalt(){
    //     byte[] saltBytes = new byte[16];
    //     using var rng = RandomNumberGenerator.Create();
    //      rng.GetBytes(saltBytes);
    //      return Convert.ToBase64String(saltBytes);
        
    // }

    public static string HashPassword(string password){
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPassword(string password ,string storedHash){
        return HashPassword(password) == storedHash;
        }

}