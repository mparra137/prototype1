using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proto1.Application.Contract;
using Proto1.Application.DTOs;
using Proto1.Application.Services;

namespace Proto1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly IPessoaService pessoaService;

    public PessoaController(IPessoaService pessoaService)
    {
        this.pessoaService = pessoaService;        
    }
    

    [HttpPost("save")]
    public async Task<IActionResult> Save(PessoaDto pessoaDto){
        try{
            var result = await pessoaService.Save(pessoaDto);
            if (result == null) return NoContent();

            return Ok(result);
        } 
        catch(Exception ex){
            return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(){
        try
        {
            var pessoas = await pessoaService.GetAll();
            if (pessoas == null) return NoContent();

            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id){
        try
        {
            var pessoa = await pessoaService.GetById(id);
            if (pessoa == null) return NoContent();

            return Ok(pessoa);
        }
        catch (Exception ex) 
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCPF(string cpf){
        try
        {
            var pessoa = await pessoaService.GetByCPFAsync(cpf);
            bool hasPreviousEntry = false;
            if (pessoa != null){
                hasPreviousEntry = true;
            }

            return Ok(new {hasEntry = hasPreviousEntry});
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Error while trying to fetch data by CPF. Error Message:" + ex.Message);
            
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        try
        {
            if(await pessoaService.Delete(id)){
                return Ok(new {message = "Person deleted"});
            }
            return BadRequest(new {message = "Unexpected error while trying to delete person"});
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
