using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class UserQuestionAnswerCollection : AuditableEntity<int>
    {
        public int QuestionSetId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int Mark { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string Answer { get; set; }
    }
}
