using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Infrastructure.Implementations.Services.Identity;

public class HashService : IHashService
{
    private const int Iterations = 200000;
    private const int KeySize = 128 / 8;
    private const KeyDerivationPrf HashAlgorithm = KeyDerivationPrf.HMACSHA256;

    public bool VerifyPassword(string password, string hash, string saltString)
    {
        var salt = Convert.FromBase64String(saltString);

        var hashToCompare = GenerateHash(password, salt);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromBase64String(hash));
    }

    public (string hash, string salt) GenerateHash(string password)
    {
        var salt = new byte[KeySize];

        RandomNumberGenerator.Fill(salt);

        var hashed = GenerateHash(password, salt);

        var hashString = Convert.ToBase64String(hashed);
        var saltString = Convert.ToBase64String(salt);

        return (hashString, saltString);
    }

    private static ReadOnlySpan<byte> GenerateHash(string password, byte[] salt)
    {
        return KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: HashAlgorithm,
            iterationCount: Iterations,
            numBytesRequested: KeySize);
    }
}