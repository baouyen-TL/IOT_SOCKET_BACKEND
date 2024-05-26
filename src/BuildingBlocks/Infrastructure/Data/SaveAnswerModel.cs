using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.Data
{
    public partial class SaveAnswerModel
    {
        public Guid UserAnswerId { get; set; }
        public Guid? QuestionId { get; set; }
        public string SelectedAnswer { get; set; }
        public int? SelectedTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual QuestionModel Question { get; set; }
    }
}
