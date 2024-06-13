using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Domain.Entities
{
    public class UpdateProfileCommand
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
