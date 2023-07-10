using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proto1.API.Models
{
    public class PaginationHeader
    {
        public int CurrentPage {get ; set;}
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}