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
        public string AnswerKey { get; set; }
        public int? TotalUserSelected { get; set; }
    }

    public class ReportTopRankingRemoteTempResponse {
        public Guid? RemoteId { get; set; }
        public Guid? AnwserId { get; set; }
    }

    public class ReportTopRankingRemoteResponse
    {
        public int? Position { get; set; }
        public string RemoteName { get; set; }
        public string UserName { get; set; }
    }

    public class ReportRankingDetailResponse
    {
        public string UserName { get; set; }
        public int? SCD { get; set; }
        public int? SCS { get; set; }
        public int? TTGC { get; set; }
    }

}
