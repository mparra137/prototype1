using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Application.DTOs;

namespace Proto1.Application.Contract;

public interface ITokenService
{
    Task<string> GenerateToken(UserCreateDto user);
}
