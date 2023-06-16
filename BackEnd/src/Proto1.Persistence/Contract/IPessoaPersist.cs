using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain;
using Proto1.Persistence.Models;

namespace Proto1.Persistence.Contract;

public interface IPessoaPersist : IGeneralPersist
{
    Task<PageList<Pessoa>> GetAllAsync(PageParams pageParams);       
    Task<Pessoa> GetByIdAsync(int pessoaId); 
    Task<Pessoa> GetByCPFAsync(string cpf);
}

