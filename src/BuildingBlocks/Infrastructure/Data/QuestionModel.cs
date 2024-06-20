using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class QuestionModel
    {
        public QuestionModel()
        {
            AnswerModels = new HashSet<AnswerModel>();
            SaveAnswerModels = new HashSet<SaveAnswerModel>();
        }

        public Guid QuestionId { get; set; }
        public Guid? TopicId { get; set; }
        public string QuestionName { get; set; }
        public int? QuestionTime { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string VideoUrl { get; set; }

        public virtual TopicModel Topic { get; set; }
        public virtual ICollection<AnswerModel> AnswerModels { get; set; }
        public virtual ICollection<SaveAnswerModel> SaveAnswerModels { get; set; }
    }
}
