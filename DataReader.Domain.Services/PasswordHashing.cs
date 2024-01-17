using DataReader.Domain.Interfaces;
using DataReader.Domain.Services.AbstractionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{

    public class PasswordHashing : IPasswordHashing
    {
        public string HashPassword(string password)
        {
            int hash = 0;
            foreach (char c in password)
            {
                hash = (hash << 5) - hash + c;
            }

            return hash.ToString("x2");
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string passwordHash = HashPassword(password);
            return passwordHash.Equals(hashedPassword);
        }
    }
}
