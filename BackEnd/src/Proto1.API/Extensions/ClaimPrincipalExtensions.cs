using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Proto1.API.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal user){
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static int GetUserId(this ClaimsPrincipal user){
        return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }

    public static string GetUserEmail(this ClaimsPrincipal user){
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }
}
