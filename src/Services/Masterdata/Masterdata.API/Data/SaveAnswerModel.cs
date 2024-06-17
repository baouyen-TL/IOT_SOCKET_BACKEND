using System;
using System.Collections.Generic;

#nullable disable

namespace Masterdata.API.Data
{
    public partial class SaveAnswerModel
    {
        public Guid SaveAnswerId { get; set; }
        public Guid? BeginGameId { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? AnswerId { get; set; }
        public Guid? RemoteId { get; set; }
        public int? SelectedTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastEditTime { get; set; }

        public virtual AnswerModel Answer { get; set; }
        public virtual BeginGameModel BeginGame { get; set; }
        public virtual QuestionModel Question { get; set; }
    }
}
