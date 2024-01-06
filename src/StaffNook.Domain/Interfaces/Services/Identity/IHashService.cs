namespace StaffNook.Domain.Interfaces.Services.Identity;

public interface IHashService
{
    bool VerifyPassword(string password, string hash, string saltString);
    (string hash, string salt) GenerateHash(string password);
}