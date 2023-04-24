using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proto1.Domain;

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }

    private string _cpf;
    public string CPF { 
        get{
            //string pattern = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$";
            //var regEx = new Regex(pattern);
            //var stringBuilder = new StringBuilder();
            //var auxCPF = stringBuilder.AppendFormat("{0}.{1}.{2}-{3}", _cpf.Substring(0,3), _cpf.Substring(3,3), _cpf.Substring(6,3), _cpf.Substring(9,2)).ToString();
            //return auxCPF;
            return _cpf;
        } 
        set{ 
            var auxCPF = value.Replace(".", "").Replace("-", "");
            _cpf = auxCPF;
        } 
    }
    public string CEP { get; set; }
    public string Endereco { get; set; }
    public int Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string UF { get; set; }
    public string Complemento { get; set; }
}
