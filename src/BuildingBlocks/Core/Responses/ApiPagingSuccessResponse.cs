using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Responses
{
    public class ApiPagingSuccessResponse : ApiSuccessResponse
    {
        public PagingResponse Paging { get; set; } = new();
    }

    public class PagingResponse
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
    }
}
