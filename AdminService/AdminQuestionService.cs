using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;

namespace AdminService
{
    public class AdminQuestionService
    {
        private compilerContext _compilerContext = new compilerContext();

        public async Task<List<Question>> GetQuestionInfo()
        {
            var context = _compilerContext;
            IQueryable<Question> questions =
                from question in context.Questions select question;
            return questions.ToList();

        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            Question questionUpdate =
                _compilerContext.Questions.First(c => c.QuestionId == question.QuestionId);

            questionUpdate.Labels = question.Labels;
            questionUpdate.Description = question.Description;
            questionUpdate.Difficulty = question.Difficulty;

            _compilerContext.SaveChanges();


            return true;
        }

        public async Task<bool> DeleteQuestion(Question question)
        {
            Question questionDelete =
               _compilerContext.Questions.First(c => c.QuestionId == question.QuestionId);

            _compilerContext.Questions.Remove(questionDelete);
            _compilerContext.SaveChanges();

            return true;
        }

        public async Task<bool> AddQuestion(Question question)
        {
            Question newQuestion = new Question();

            newQuestion.Labels = question.Labels;
            newQuestion.Difficulty = question.Difficulty;
            newQuestion.Description = question.Description;

            _compilerContext.Questions.Add(newQuestion);
            _compilerContext.SaveChanges();

            return true;
        }

    }
}