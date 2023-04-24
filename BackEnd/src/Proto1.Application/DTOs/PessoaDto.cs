using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proto1.Application.DTOs;

public class PessoaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string CEP { get; set; }
    public string Endereco { get; set; }
    public int Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string UF { get; set; }
    public string Complemento { get; set; }
}
