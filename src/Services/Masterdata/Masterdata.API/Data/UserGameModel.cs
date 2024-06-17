using System;
using System.Collections.Generic;

#nullable disable

namespace Masterdata.API.Data
{
    public partial class UserGameModel
    {
        public Guid UserGameId { get; set; }
        public Guid? BeginGameId { get; set; }
        public Guid? RemoteId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual BeginGameModel BeginGame { get; set; }
        public virtual RemoteModel Remote { get; set; }
    }
}
