using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class QuestionSetCollection : AuditableEntity<int>
    {
        public int QuestionSetId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int Mark { get; set; }
    }
}
