using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Infrastructure.Utilities
{
    public static class UuidGenerator
    {
        public static Guid GenerateUuid()
        {
            return Guid.NewGuid();
        }
    }
}

