using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Domain.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace Application.Service
{
    public interface IPasswordService
    {
        public Task<ClaimsIdentity> GetIdentityAsync( string login, string password );
        public string HashPassword( string password, RandomNumberGenerator rng );
    }
    public class PasswordService : IPasswordService
    {
        const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
        const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
        const int SaltSize = 128 / 8; // 128 bits

        private readonly IUserRepository _userRepository;
        public PasswordService( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync( string login, string password )
        {
            User user = await _userRepository.GetByLoginAsync( login );
            if ( user != null && VerifyHashedPassword( Convert.FromBase64String( user.Password ), password ) )
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity( claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType );
                return claimsIdentity;
            }

            return null;
        }


        public string HashPassword( string password, RandomNumberGenerator rng )
        {
            byte[] salt = new byte[ SaltSize ];
            rng.GetBytes( salt );
            byte[] subkey = KeyDerivation.Pbkdf2( password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength );

            var outputBytes = new byte[ 1 + SaltSize + Pbkdf2SubkeyLength ];
            Buffer.BlockCopy( salt, 0, outputBytes, 1, SaltSize );
            Buffer.BlockCopy( subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength );

            return Convert.ToBase64String( outputBytes );
        }

        private bool VerifyHashedPassword( byte[] hashedPassword, string password )
        {
            if ( hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength )
            {
                return false;
            }

            byte[] salt = new byte[ SaltSize ];
            Buffer.BlockCopy( hashedPassword, 1, salt, 0, salt.Length );

            byte[] expectedSubkey = new byte[ Pbkdf2SubkeyLength ];
            Buffer.BlockCopy( hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length );

            byte[] actualSubkey = KeyDerivation.Pbkdf2( password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength );

            return CryptographicOperations.FixedTimeEquals( actualSubkey, expectedSubkey );
        }
    }
}
