using Core.Exceptions;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.DTOs.Question;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masterdata.Application.Features.V1.Queries.Question
{
    public interface IQuestionQuery
    {
        public Task<QuestionResponse> GetListQuestionByTopicId(Guid TopicId);
    }
    public class QuestionQuery : IQuestionQuery
    {
        private readonly IOT_SOCKETContext _context;

        public QuestionQuery(IOT_SOCKETContext context) {
            _context = context;
        }
        public async Task<QuestionResponse> GetListQuestionByTopicId(Guid TopicId)
        {
            var QuestionRes = new QuestionResponse();

            var topic = await _context.TopicModels.FirstOrDefaultAsync(x => x.TopicId == TopicId);
            if (topic == null) throw new BadRequestException("TopicId này không tồn tại!");

            // Response Topic
            QuestionRes.TopicId = TopicId;
            QuestionRes.TopicName = topic.TopicName;

            var listQuestions = await _context.QuestionModels.Where(x => x.TopicId == TopicId)
                .Include(x => x.AnswerModels)
                .OrderBy(x => x.CreateTime)
                .ToListAsync();

            // Response ListQuestion
            var ListQuestionDataRes = new List<QuestionDataResponse>();
            foreach (var question in listQuestions)
            {
                var QuestionDataRes = new QuestionDataResponse
                {
                    QuestionId = question.QuestionId,
                    QuestionName = question.QuestionName,
                    QuestionTime = question.QuestionTime,
                };

                // Response List Answer
                var ListAnswerRes = new List<AnswerResponse>();
                foreach (var answer in ListQuestionDataRes)
                {
                    var answerInfo = question.AnswerModels.Where(x => x.QuestionId == question.QuestionId).FirstOrDefault();
                    var answerRes = new AnswerResponse
                    {
                        AnswerId = answerInfo.AnswerId,
                        AnswerName = answerInfo.AnswerName,
                        IsCorrect = answerInfo.IsCorrect,
                    };

                    ListAnswerRes.Add(answerRes);
                }
                // Response List Answer in Question
                QuestionDataRes.ListAnswerDatas = ListAnswerRes;
                // Response List Question in Topic
                ListQuestionDataRes.Add(QuestionDataRes);
            };

            QuestionRes.ListQuestionDatas = ListQuestionDataRes;

            return QuestionRes;
        }
    }
}
