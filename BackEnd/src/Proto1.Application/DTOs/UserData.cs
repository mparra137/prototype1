using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain.Identity;

namespace Proto1.Application.DTOs;

public class UserData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName {get ; set;}
    public string UserName { get; set; }
    public string Email { get; set;}

    public IEnumerable<string> Roles{ get ; set; }
}
