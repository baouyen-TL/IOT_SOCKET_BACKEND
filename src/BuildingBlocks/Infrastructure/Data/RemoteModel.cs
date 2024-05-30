using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class RemoteModel
    {
        public RemoteModel()
        {
            UserGameModels = new HashSet<UserGameModel>();
        }

        public Guid RemoteId { get; set; }
        public string RemoteName { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<UserGameModel> UserGameModels { get; set; }
    }
}
