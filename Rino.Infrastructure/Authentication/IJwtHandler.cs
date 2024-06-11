using Rino.Domain.Entities;
using Rino.Infrastructure.Authentication;
using Rino.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Domain.Services
{
    public interface IJwtHandler
    {
        string GenerateToken(User user);
    }

}
