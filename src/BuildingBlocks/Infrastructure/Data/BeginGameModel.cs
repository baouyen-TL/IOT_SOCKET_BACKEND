using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class BeginGameModel
    {
        public BeginGameModel()
        {
            SaveAnswerModels = new HashSet<SaveAnswerModel>();
            UserGameModels = new HashSet<UserGameModel>();
        }

        public Guid BeginGameId { get; set; }
        public Guid? TopicId { get; set; }
        public string ClassName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual TopicModel Topic { get; set; }
        public virtual ICollection<SaveAnswerModel> SaveAnswerModels { get; set; }
        public virtual ICollection<UserGameModel> UserGameModels { get; set; }
    }
}
