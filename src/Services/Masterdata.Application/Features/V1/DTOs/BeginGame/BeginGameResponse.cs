using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.DTOs.BeginGame
{
    public class BeginGameResponse
    {
        public int? STT { get; set; }
        public Guid? BeginGameId { get; set; }
        public string TopicName { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
