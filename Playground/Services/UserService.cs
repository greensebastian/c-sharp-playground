using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Playground.Models.User;

namespace Playground
{
    public class UserService
    {
        private readonly UserManager<PlaygroundUser> _userManager;
        private readonly SignInManager<PlaygroundUser> _signInManager;
        private PlaygroundUser _currentUser = null;
        public UserService(UserManager<PlaygroundUser> userManager, SignInManager<PlaygroundUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<PlaygroundUser> Login(string username, string password)
        {
            return await SignIn(username, password);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task LogoutAndDeleteCurrent(string password)
        {
            var user = await CurrentUser();
            if(CorrectPassword(password, user))
            {
                await Logout();
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<(IdentityResult, PlaygroundUser)> Register(string username, string password, string email, bool signIn)
        {
            var salt = CreateSalt();
            var hash = CreateHash(password, salt);
            var user = new PlaygroundUser
            {
                Id = username,
                UserName = username,
                PasswordSalt = salt,
                PasswordHash = hash,
                Email = email
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return (result, null);

            if (signIn) await SignIn(username, password);
            return (result, user);
        }

        private async Task<PlaygroundUser> SignIn(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (CorrectPassword(password, user))
                {
                    await _signInManager.SignInAsync(user, true);
                    _currentUser = user;
                }
            }
            return _currentUser;
        }

        private static bool CorrectPassword(string password, PlaygroundUser user)
        {
            return CreateHash(password, user.PasswordSalt) == user.PasswordHash;
        }

        public async Task<PlaygroundUser> CurrentUser()
        {
            if (_currentUser == null)
            {
                var principal = _signInManager.Context.User;
                _currentUser = await _userManager.GetUserAsync(principal);
            }
            return _currentUser;
        }

        // Generate a random string with a given size
        public static string CreateSalt(int bitCount = 128)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[bitCount / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Encoding.UTF8.GetString(salt);
        }

        public static string CreateHash(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
