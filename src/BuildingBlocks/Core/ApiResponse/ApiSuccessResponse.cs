using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiResponse
{
    public class ApiSuccessResponse
    {
        public int Code { get; set; } = 200;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
