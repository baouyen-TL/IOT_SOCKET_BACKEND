using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.DTOs.Topic
{
    public class TopicResponse
    {
        public int? STT { get; set; }
        public Guid? TopicId { get; set; }
        public string TopicName { get; set; }
        public int? CountQuestion { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
