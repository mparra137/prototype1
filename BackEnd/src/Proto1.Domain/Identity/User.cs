using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Proto1.Domain.Identity;

public class User : IdentityUser<int>
{
    public string Name { get; set; }
    public string LastName { get; set; }

    public IEnumerable<UserRole> UserRoles {get;set;}
    
}
