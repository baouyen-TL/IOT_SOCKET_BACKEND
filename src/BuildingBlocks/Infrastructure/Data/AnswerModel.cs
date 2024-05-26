using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class AnswerModel
    {
        public Guid AnswerId { get; set; }
        public Guid? QuestionId { get; set; }
        public string AnswerName { get; set; }
        public bool? IsCorrect { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual QuestionModel Question { get; set; }
    }
}
