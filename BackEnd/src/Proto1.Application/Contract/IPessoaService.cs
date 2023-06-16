using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Application.DTOs;
using Proto1.Persistence.Models;

namespace Proto1.Application.Contract;

public interface IPessoaService
{
    Task<PessoaDto> Save(PessoaDto pessoaDto); 
    Task<bool> Delete(int pessoaId);
    Task<PageList<PessoaDto>> GetAllAsync(PageParams pageParams);
    Task<PessoaDto> GetById(int pessoaId);

    Task<PessoaDto> GetByCPFAsync(string cpf);
}
