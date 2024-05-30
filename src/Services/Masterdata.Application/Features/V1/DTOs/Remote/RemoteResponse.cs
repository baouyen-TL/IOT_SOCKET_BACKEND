using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.DTOs.Remote
{
    public class RemoteResponse
    {
        public Guid? RemoteId { get; set; }
        public string RemoteName { get; set; }
        public bool? Status { get; set; }
    }
}
