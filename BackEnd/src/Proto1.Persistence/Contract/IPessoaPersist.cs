using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain;

namespace Proto1.Persistence.Contract;

public interface IPessoaPersist : IGeneralPersist
{
    Task<List<Pessoa>> GetPessoasAsync();       
    Task<Pessoa> GetByIdAsync(int pessoaId); 
    Task<Pessoa> GetByCPFAsync(string cpf);
}

