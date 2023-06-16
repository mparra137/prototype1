using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proto1.Application.Contract;
using Proto1.Application.DTOs;
using Proto1.Application.Services;
using Proto1.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Proto1.Persistence.Models;

namespace Proto1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoaController : ControllerBase
{
    private readonly IPessoaService pessoaService;
    private readonly IUtil util;

    public PessoaController(IPessoaService pessoaService, IUtil util)
    {
            this.util = util;
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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams){
        try
        {
            var pessoas = await pessoaService.GetAllAsync(pageParams);
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

    [HttpGet("checkcpf/{cpf}")]
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

    [HttpGet("report/all")]
    public async Task<IActionResult> GeneratePDFReport([FromQuery] PageParams pageParams){
        try
        {
            var ListaPessoas = await pessoaService.GetAllAsync(pageParams);   

            //await util.createTxtFile();
            var pdfFile = await util.createPdfFile(ListaPessoas);
            Response.Headers.Add("Access-Control-Expose-Headers", "content-disposition");
            return File(pdfFile, "application/octet-stream", "pessoas.pdf");    
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
        
    }

    [HttpGet("teste")]
    public IActionResult Teste(){
        string result = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string result2 = DateTime.UtcNow.ToLocalTime().ToString("yyyyMMddHHmmssfff");

        return Ok(new {
            UTCTime = result,
            LocalTime = result2
        });
    }
}
