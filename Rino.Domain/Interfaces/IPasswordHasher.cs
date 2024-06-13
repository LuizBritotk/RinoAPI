using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string UnmaskPassword(string maskedPassword);
        string MaskPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
