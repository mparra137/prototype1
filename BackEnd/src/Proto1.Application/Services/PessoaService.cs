using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Proto1.Application.Contract;
using Proto1.Application.DTOs;
using Proto1.Domain;
using Proto1.Persistence;
using Proto1.Persistence.Contract;

namespace Proto1.Application.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaPersist pessoaPersist;
    private readonly IMapper mapper;

    public PessoaService(IPessoaPersist pessoaPersist, IMapper mapper)
    {
        this.mapper = mapper;
        this.pessoaPersist = pessoaPersist;        
    }

    public async Task<PessoaDto> Save(PessoaDto pessoaDto)
    {
        try
        {            
            var pessoa = mapper.Map<Pessoa>(pessoaDto);
            if (pessoaDto.Id == 0){                
                pessoaPersist.Insert(pessoa);
            } else {                
                pessoaPersist.Update(pessoa);
            }
            if (await pessoaPersist.SaveChangesAsync()) {
                return mapper.Map<PessoaDto>(pessoa);
            }      
            return null;
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to save person: " + ex.Message);
        }
    }
    public async Task<bool> Delete(int pessoaId)
    {
        try
        {
            var pessoa = await pessoaPersist.GetByIdAsync(pessoaId);
            if (pessoa == null) return false;

            pessoaPersist.Delete(pessoa);
            return (await pessoaPersist.SaveChangesAsync());
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to delete a person: " + ex.Message);
        }
    }

    public async Task<List<PessoaDto>> GetAll()
    {
        try
        {
            var pessoas = await pessoaPersist.GetPessoasAsync();
            return mapper.Map<List<PessoaDto>>(pessoas);
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to get the list of persons: " +ex.Message);
        }
    }

    public async Task<PessoaDto> GetById(int pessoaId){
        try
        {
            var pessoa = await pessoaPersist.GetByIdAsync(pessoaId);
            if (pessoa == null) return null;

            return mapper.Map<PessoaDto>(pessoa);
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to get person: " + ex.Message);
        }
    }

    public async Task<PessoaDto> GetByCPFAsync(string cpf){
        try
        {
            var pessoa = await pessoaPersist.GetByCPFAsync(cpf);
            if (pessoa == null) return null;
            
            return mapper.Map<PessoaDto>(pessoa);
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
    }

    
}
