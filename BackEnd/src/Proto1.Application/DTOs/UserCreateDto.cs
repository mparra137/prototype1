using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proto1.Application.DTOs;

public class UserCreateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName {get ; set;}
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set;}
}
