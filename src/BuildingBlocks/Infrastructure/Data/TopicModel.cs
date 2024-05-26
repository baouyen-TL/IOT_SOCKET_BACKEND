using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class TopicModel
    {
        public TopicModel()
        {
            BeginGameModels = new HashSet<BeginGameModel>();
            QuestionModels = new HashSet<QuestionModel>();
        }

        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual ICollection<BeginGameModel> BeginGameModels { get; set; }
        public virtual ICollection<QuestionModel> QuestionModels { get; set; }
    }
}
