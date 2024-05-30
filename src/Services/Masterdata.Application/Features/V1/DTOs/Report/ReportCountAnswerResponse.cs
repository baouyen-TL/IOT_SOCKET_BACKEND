using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.DTOs.Report
{
    public class ReportCountAnswerResponse
    {
        public Guid? AnswerId { get; set; }
        public string AnswerName { get; set; }
        public int? TotalUserSelected { get; set; }
    }
}
