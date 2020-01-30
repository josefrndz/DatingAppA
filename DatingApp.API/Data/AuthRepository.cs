using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext Context;

        public AuthRepository(DataContext context)
        {
            this.Context = context;
        }

        // We use the password parameter and calculate the hash, which we will use to compare
        // against the hashed codes in our database.
        public async Task<User> Login(string username, string password)
        {
            var user = await this.Context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) 
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            // Here we are using the keywork out to explicitly pass in REFERENCES to passwordHash
            // and passwordSalt so that when they are updated inside our CreatePassordHash method
            // then they'll also be created inside here as well.
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this.Context.Users.AddAsync(user);
            await this.Context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {   
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await this.Context.Users.AnyAsync(x => x.Username == username))
                return true;
            
            return false;
        }
    }
}