using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CourseAll.API.Models;

namespace CourseAll.API.Helpers
{
    public static class Helper
    {
        public static string GetRole(string header)
        {
            var jwt = header.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        }

        public static int GetId(string header)
        {
            var jwt = header.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return Convert.ToInt32(token.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        }

        public static bool IsValidPassword(string rawData, byte[] passwordHash)
        {
            var currentHash = ComputeHash(rawData);

            for(int i = 0; i < currentHash.Length; i++)
            {
                if(currentHash[i] != passwordHash[i]) return false;
            }

            return true;
        }
        public static byte[] ComputeHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())  
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));                
                return bytes;
            }
        }
    }
}