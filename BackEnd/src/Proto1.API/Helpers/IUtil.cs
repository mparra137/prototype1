using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Application.DTOs;

namespace Proto1.API.Helpers;

public interface IUtil
{
    Task createTxtFile(string text);  
    Task<Byte[]> createPdfFile(List<PessoaDto> lista);
}
